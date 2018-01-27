using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    private const float INITIAL_SPEED = 5f;
    private const float KICK_TIME_MAX = 1f;

    [SerializeField] private float _weight = 1f;
    [SerializeField] private AnimationCurve _speedCurve = new AnimationCurve();

    private bool _isKicked = false;
    private Vector3 _direction = Vector3.zero;
    private float _speed = INITIAL_SPEED;
    private float _kickSpeed = INITIAL_SPEED;
    private float _kickTimer = 0f;

    public float rotationSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);

        if(_isKicked == true)
        {
            Move();
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

    public void ActiveItem(PlayerController player) {
        Debug.Log(player.name + " activate item " + this.name);
        gameObject.SetActive(false);
    }

    private void Move() {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    public void Kick(Vector3 direction, float strengh) {
        _direction = direction;
        _kickSpeed = strengh / _weight;
        _isKicked = true;
        _kickTimer = 0f;
    }
}
