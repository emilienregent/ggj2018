using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IFrequency
{
    private const float INITIAL_SPEED = 5f;
    private const float KICK_TIME_MAX = 1f;

	[SerializeField] protected PlayerController   _player = null;
	[SerializeField] protected float              _weight = 1f;
	[SerializeField] protected AnimationCurve     _speedCurve = new AnimationCurve();

	protected bool _isKicked = false;
	protected bool _isMoving = false;

	protected Vector3 _direction  = Vector3.zero;
	protected float   _speed      = INITIAL_SPEED;
	protected float   _kickSpeed  = INITIAL_SPEED;
	protected float   _kickTimer  = 0f;

	protected NavMeshAgent _agent	= null;

	protected RoomSpawner _roomSpawner = null;

	public float dmg = 0f;
	public float hitFrequency = 0f;
	protected bool _canHit = false;
	protected float _timeToHit = 0f;

	private PatrolWaypoint _patrolWaypoint = null;

	public Frequency frequency { get; set; }

	public float distanceToAttack = 0f;

	protected bool _huntPlayer = true;

	public Attack _attack = null;

    private void Start()
    {
		_agent = GetComponent<NavMeshAgent> ();
		_attack = GetComponent<Attack> ();

		MoveToPlayer ();
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

    // TODO : Patrol move not works really well.
    protected virtual void MoveToPlayer()
    {
		if (_player.isDead == false)
		{
			_huntPlayer = true;
			_agent.stoppingDistance = distanceToAttack;
			_agent.destination = _player.transform.position;
		}

		SetCanHit(_agent.remainingDistance < _agent.stoppingDistance);
    }

	protected void MoveToPatrol ()
	{
		if (_huntPlayer == true)
		{
			_huntPlayer = false;

			_agent.stoppingDistance = 1f;
			_patrolWaypoint = _roomSpawner.GetPatrolPoint (this, -1);
		}

		_agent.destination = _patrolWaypoint.transform.position;

		if (_agent.remainingDistance < _agent.stoppingDistance)
		{
			_patrolWaypoint = _roomSpawner.GetPatrolPoint (this, _patrolWaypoint.index);
		}
	}

	protected void SetCanHit (bool canHit)
	{
		_canHit = canHit;
	}

	protected void LaunchAttack ()
	{
		_attack.Launch (this, _player);
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
			if (_player.isDead == false)
				MoveToPlayer ();
			else
				MoveToPatrol ();
        }

		if (_canHit == true)
		{
			if (Time.realtimeSinceStartup > _timeToHit)
			{
				_timeToHit = Time.realtimeSinceStartup + hitFrequency;
				LaunchAttack ();
			}	
		}
    }

	public void HitByBullet (BulletController bullet)
	{
		_roomSpawner.MonsterKill (this);
		GameObject.Destroy (this.gameObject);
	}
}