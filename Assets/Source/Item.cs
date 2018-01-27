using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Item : MonoBehaviour {
    private const float INITIAL_SPEED = 5f;
    private const float KICK_TIME_MAX = 1f;

    [SerializeField] private float _weight = 1f;
    [SerializeField] private AnimationCurve _speedCurve = new AnimationCurve();

    private bool _isKicked = false;
    public bool canBeKick = true;
    private Vector3 _direction = Vector3.zero;
    private float _speed = INITIAL_SPEED;
    private float _kickSpeed = INITIAL_SPEED;
    private float _kickTimer = 0f;

    public float rotationSpeed;

    public abstract void ActiveItem(PlayerController player);

    protected void Move() {
        if(canBeKick && _isKicked == true)
        {
            transform.position += _direction * _speed * Time.deltaTime;
            _speed = _kickSpeed * _speedCurve.Evaluate(_kickTimer);
            _isKicked = _speed > 0f;
            _kickTimer += Time.deltaTime;

            // Just stop to be kicked out
            if(_isKicked == false)
            {
                // Reset speed
                _speed = INITIAL_SPEED / _weight;
            }
        }
    }

    public void Kick(Vector3 direction, float strengh) {
        _direction = direction;
        _kickSpeed = strengh / _weight;
        _isKicked = true;
        _kickTimer = 0f;
    }
}
