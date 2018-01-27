using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        _isMoving = true;

        MoveToPlayer();
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
        // Update direction to player
        Vector3 heading = _player.transform.position - transform.position;
        float sqrLen = heading.sqrMagnitude;

        if (sqrLen > 1f)
        {
            float distance = heading.magnitude;

            _direction = heading / distance;

            Move();
        }
        else
        {
            _isMoving = false;
        }
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
        else if (_isMoving == true)
        {
            MoveToPlayer();
        }
    }
}