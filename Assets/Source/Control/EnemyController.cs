﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IFrequency
{
    private const float INITIAL_SPEED = 5f;

    private PlayerController   _player = null;
    [SerializeField] private float              _weight = 1f;
    [SerializeField] private AnimationCurve     _speedCurve = new AnimationCurve();

    private int _currentDungeonId = 0;

    private bool _isKicked = false;
    private bool _isMoving = false;
    private bool _isWarping = false;
    private DirectionEnum _warpingDirection = DirectionEnum.NONE;

    private Vector3 _direction          = Vector3.zero;
    private float   _sqrDistToPlayer    = 0f;
    private float   _speed      = INITIAL_SPEED;
    private float   _kickSpeed  = INITIAL_SPEED;
    private float   _kickTimer  = 0f;

	private NavMeshAgent _agent	= null;

	private RoomSpawner _roomSpawner = null;

	public float dmg = 0f;
	public float hitFrequency = 0f;
	private bool _canHit = false;
	private float _timeToHit = 0f;

	public Frequency frequency { get; set; }

    private void Start()
    {
		_agent = GetComponent<NavMeshAgent> ();

        _isMoving = true;

        MoveToPlayer();
    }

	public void SetPlayer (PlayerController playerController)
	{
        if (_player != null)
        {
            _player.RemoveEnemy(this);
        }

		_player = playerController;
        _player.AddEnemy(this);

        _currentDungeonId = _player.dungeon.id;

        //UnityEngine.Debug.Log("Spawn enemy " + this + " for player " + _player.playerId + " for dungeon " + _player.dungeon);
	}

	public void SetRoomSpawner (RoomSpawner roomSpawner)
	{
		_roomSpawner = roomSpawner;
	}

    public void Kick(Vector3 direction, float strengh)
    {
        //UnityEngine.Debug.Log("Kicked by player " + _player.playerId + " in dungeon " + _player.dungeon);
        _direction  = direction;
        _kickSpeed  = strengh / _weight;
        _isKicked   = true;
        _kickTimer  = 0f;
    }

    // TODO : Remove as soon as we have pathfinding for enemies
    private void MoveToPlayer()
    {
		_agent.destination = _player.transform.position;
		_isMoving = _agent.remainingDistance < _agent.stoppingDistance;
    }

    private void Move()
    {
        transform.position += _direction * _speed * Time.deltaTime;

        if (_isKicked == true)
        {
            if (_player.dungeon == null)
            {
                UnityEngine.Assertions.Assert.IsNotNull(_player, "Player is null when kicking enemy " + this);
                UnityEngine.Assertions.Assert.IsNotNull(_player.dungeon, "Player " + _player.playerId + " doesn't have a dungeon when kicking enemy " + this);
                UnityEngine.Assertions.Assert.IsNotNull(_player.dungeon.game, "Dungeon " + _player.dungeon.id + " doesn't have a game reference when kicking enemy " + this);
            }
            DirectionEnum direction = _player.dungeon.game.GetDirection(_currentDungeonId, transform.position);

            if (_player.dungeon.game.IsAvailableDirection(_currentDungeonId, direction) == false)
            {
                StopKick();
            }
            else
            {
                if (_isWarping == true && direction == DirectionEnum.NONE)
                {
                    _isWarping = false;
                }
                else if (_isWarping == false && direction != DirectionEnum.NONE)
                {
                    int dungeonId = _player.dungeon.game.GetDungeonId(_currentDungeonId, direction);
                    Vector3 newPosition = _player.dungeon.game.GetPosition(transform.position, direction, _currentDungeonId, dungeonId);

                    // Froze warping direction
                    _isWarping = true;
                    _warpingDirection = direction;

                    // Set new player and teleport into new room
                    _player = _player.dungeon.game.GetPlayer(dungeonId);
                    _currentDungeonId = dungeonId;
                    transform.position = newPosition;
                    _agent.destination = _player.transform.position;
                }
            }
        }
    }

    private void UpdateDirectionToPlayer()
    {
        // Update direction to player
        Vector3 heading = _player.transform.position - transform.position;
        float distance = heading.magnitude;

        _sqrDistToPlayer = heading.sqrMagnitude;

        _direction = heading / distance;
    }

    private void Update()
    {
        if (_isKicked == true)
        {
            Move();

            if (_isKicked == true)
            {
                _speed = _kickSpeed * _speedCurve.Evaluate(_kickTimer);
                _kickTimer += Time.deltaTime;
            }

            // Just stop to be kicked out if reach 0
            if (_speed <= 0f)
            {
                StopKick();
            }
        }
        else
        {
            MoveToPlayer();
        }

		if (_canHit == true)
		{
			if (Time.realtimeSinceStartup > _timeToHit)
			{
				_timeToHit = Time.realtimeSinceStartup + hitFrequency;
				_player.Hit (dmg);
			}	
		}
    }


	public void HitByBullet (BulletController bullet)
	{
        if (_player != null)
        {
            _player.RemoveEnemy(this);
        }

		_roomSpawner.MonsterKill (this);
		GameObject.Destroy (this.gameObject);
	}

	void OnTriggerEnter (Collider collider)
	{
		if (collider.tag == "player")
		{
			_canHit = true;
			_timeToHit = Time.realtimeSinceStartup;
		}
	}

	void OnTriggerExit (Collider collider)
	{
		if (collider.tag == "player")
		{
			_canHit = false;
		}
	}

    private void StopKick()
    {
        //UnityEngine.Debug.Log("Stop kick ");

        // Reset speed
        _speed      = INITIAL_SPEED / _weight;
        _isKicked   = false;
        _isMoving   = true;
        _isWarping  = false;
    }
}