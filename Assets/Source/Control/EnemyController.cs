using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour 
{
    private const float INITIAL_SPEED = 5f;
    private const float KICK_TIME_MAX = 1f;

    [SerializeField] private PlayerController   _player = null;
    [SerializeField] private float              _weight = 1f;
    [SerializeField] private AnimationCurve     _speedCurve = new AnimationCurve();

    private bool _isKicked = false;
    private bool _isMoving = false;

    private Vector3 _direction  = Vector3.zero;
    private float   _speed      = INITIAL_SPEED;
    private float   _kickSpeed  = INITIAL_SPEED;
    private float   _kickTimer  = 0f;

	private NavMeshAgent _agent	= null;

	private RoomSpawner _roomSpawner = null;

	public float dmg = 0f;
	public float hitFrequency = 0f;
	private bool _canHit = false;
	private float _timeToHit = 0f;

    private void Start()
    {
		_agent = GetComponent<NavMeshAgent> ();

        _isMoving = true;

        MoveToPlayer();
    }

	public void SetPlayer (PlayerController playerController)
	{
		_player = playerController;
	}

	public void SetRoomSpawner (RoomSpawner roomSpawner)
	{
		_roomSpawner = roomSpawner;
	}

    public void Kick(Vector3 direction, float strengh)
    {
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
    }

    private void Update()
    {
        if (_isKicked == true)
        {
            Move();

            _speed  = _kickSpeed * _speedCurve.Evaluate(_kickTimer);
            _isKicked = _speed > 0f;
            _kickTimer += Time.deltaTime;

            // Just stop to be kicked out
            if (_isKicked == false)
            {
                // Reset speed
                _speed      = INITIAL_SPEED / _weight;
                _isMoving   = true;
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
}