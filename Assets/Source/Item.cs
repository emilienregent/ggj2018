using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private bool _isWarping = false;
    private DirectionEnum _warpingDirection = DirectionEnum.NONE;
    public float rotationSpeed;
    private PlayerController _player = null;
    private int _currentDungeonId = 0;
    protected NavMeshAgent _agent = null;

    public abstract void ActiveItem(PlayerController player);


    public void SetPlayer(PlayerController playerController) {
        _player = playerController;

        _currentDungeonId = _player.dungeon.id;
    }

    protected void Move() {
        if(canBeKick && _isKicked == true)
        {
            transform.position += _direction * _speed * Time.deltaTime;

            if(_player.dungeon == null)
            {
                UnityEngine.Assertions.Assert.IsNotNull(_player, "Player is null when kicking enemy " + this);
                UnityEngine.Assertions.Assert.IsNotNull(_player.dungeon, "Player " + _player.playerId + " doesn't have a dungeon when kicking enemy " + this);
                UnityEngine.Assertions.Assert.IsNotNull(_player.dungeon.game, "Dungeon " + _player.dungeon.id + " doesn't have a game reference when kicking enemy " + this);
            }

            DirectionEnum direction = _player.dungeon.game.GetDirection(_currentDungeonId, transform.position);
           
            if(_player.dungeon.game.IsAvailableDirection(_currentDungeonId, direction) == false)
            {
                StopKick();
            } else
            {
                if(_isWarping == true && direction == DirectionEnum.NONE)
                {
                    _isWarping = false;
                } else if(_isWarping == false && direction != DirectionEnum.NONE)
                {
                    int dungeonId = _player.dungeon.game.GetDungeonId(_currentDungeonId, direction);
                    Vector3 newPosition = _player.dungeon.game.GetPosition(transform.position, direction, _currentDungeonId, dungeonId);
                    
                    // Froze warping direction
                    _isWarping = true;
                    _warpingDirection = direction;

                    // Set new player and teleport into new room
                    _agent.Warp(newPosition);
                }
            }
        }
    }

    protected void StopKick() {
        //UnityEngine.Debug.Log("Stop kick ");

        // Reset speed
        _speed = INITIAL_SPEED / _weight;
        _isKicked = false;
        _isWarping = false;
    }

    public void Kick(Vector3 direction, float strengh) {
        _direction = direction;
        _kickSpeed = strengh / _weight;
        _isKicked = true;
        _kickTimer = 0f;
    }
}
