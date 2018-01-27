﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IFrequency {
    
    private float defaultSpeed;
    private float defaultAcceleration;
    
    public float maxDashTime = 1.0f;
    public float dashStoppingSpeed = 0.1f;
    public float dashSpeedMultiplicator = 4f;
    public float dashAccelerationMultiplicator = 12f;
    private float currentDashTime;
    public Vector3 dashDirection = Vector3.zero;

    public bool isDashing = false;
    public bool canDash = true;

    public float triggerDeadZone = 0.5f;
    public float sightAngle = 25f;
    public int playerId;
    public  float                strengh = 10f;   
    public  float                kickDistance = 2f;
    private float                _kickDistanceSqr = 0f;
    private EnemyController[]    _enemies = null;

    public  AudioClip   sfxKickSuccess;
    public  AudioClip   sfxKickFail;
    private AudioSource sfxAudioSource;

    public GunController gun;
    public Camera playerCamera;
    private Vector3 cameraOffset;

	public float fullHp = 0f;
	private float _hp = 0f;
	private NavMeshAgent _agent = null;

	public Frequency frequency { get; set; }

	public Dungeon dungeon;

	public bool isDead = false;

    // Use this for initialization

    private void Start () {
		_agent = GetComponent<NavMeshAgent> ();
        defaultSpeed = _agent.speed;
        defaultAcceleration = _agent.acceleration;

        _kickDistanceSqr = kickDistance * kickDistance;

        // TOOO : Remove as soon as we have an enemy spawner
        _enemies = FindObjectsOfType<EnemyController>() as EnemyController[];

		_hp = fullHp;

        // Get the camera offset
        cameraOffset = playerCamera.transform.position - transform.position;
        // Sound
        sfxAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update () {

        // MOVE
        if(isDashing == false)
        {
            Move();
        } else
        {
            UpdateDash();
        }
        
        Rotate();
       
        // FIRE
        gun.isFiring = (Input.GetAxis("Player_" + playerId + "_Fire1") >= triggerDeadZone);

        // DASH
        if(Input.GetAxis("Player_" + playerId + "_Dash") >= triggerDeadZone && canDash)
        {
            Dash();
        }

        if(Input.GetAxis("Player_" + playerId + "_Dash") < triggerDeadZone && !isDashing && !canDash)
        {
            canDash = true;
        }

        // ACTIONS
        if(Input.GetButtonDown("Player_" + playerId + "_Fire2"))
        {
            List<Item> closeItems = GetInFrontItems();
            if(closeItems.Count > 0)
            {
                GetInFrontItems()[0].ActiveItem(this);
            }
          
            Debug.Log("SPELL 2");
        }

        if(Input.GetButtonDown("Player_" + playerId + "_Fire3"))
        {
            Kick();

            // /!\ TODO : Made generic area detection/kick /!\ 
            List<Item> closeItems = GetInFrontItems();
            if(closeItems.Count > 0)
            {
                Item frontItem = GetInFrontItems()[0];
                Vector3 heading = frontItem.transform.position - transform.position;
                float distance = heading.magnitude;
                Vector3 direction = heading / distance;

                frontItem.Kick(direction, strengh);
            }
        }

    }

    private void LateUpdate()
    {
        playerCamera.transform.position = transform.position + cameraOffset;
    }

    // get input from the left stick for player movement
    private void Move() 
	{
		Vector3 heading = new Vector3(Input.GetAxis("Player_" + playerId + "_Horizontal"), 0, Input.GetAxis("Player_" + playerId + "_Vertical"));
		_agent.destination = transform.position + heading.normalized;
    }

    // get input from the left stick for player movement
    private void Dash() {
        if(isDashing == false && canDash == true)
        {
            currentDashTime = 0.0f;
            isDashing = true;
            canDash = false;
            dashDirection = new Vector3(Input.GetAxis("Player_" + playerId + "_Horizontal"), 0, Input.GetAxis("Player_" + playerId + "_Vertical"));
           
            _agent.speed *= dashSpeedMultiplicator;
            _agent.acceleration *= dashAccelerationMultiplicator;
            
        }
     
    }

    private void UpdateDash() {
        if(isDashing == false) { return; }
        if(currentDashTime < maxDashTime)
        {
            currentDashTime += dashStoppingSpeed;
            _agent.destination = transform.position + dashDirection.normalized;
        } else
        {
            resetSpeedAndAcceleration();

        }
        
    }

    private void resetSpeedAndAcceleration() {
        _agent.velocity = Vector3.zero;
        _agent.speed = defaultSpeed;
        _agent.acceleration = defaultAcceleration;
        isDashing = false;
        dashDirection = Vector3.zero;
    }

    // get input from right stick for the player rotation
    private void Rotate() {
        Vector3 direction = Vector3.right * Input.GetAxisRaw("Player_" + playerId + "_RotationH") + Vector3.forward * -Input.GetAxisRaw("Player_" + playerId + "_RotationV");
        if(direction.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    private void Kick()
    {
        List<EnemyController> closeEnemies = GetCloseEnemies();

        for (int i = 0; i < closeEnemies.Count; ++i)
        {
            Vector3 heading = closeEnemies[i].transform.position - transform.position;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;

            closeEnemies[i].Kick(direction, strengh);
        }

        if(closeEnemies.Count > 0)
        {
            sfxAudioSource.clip = sfxKickSuccess;
        } else
        {
            sfxAudioSource.clip = sfxKickFail;
        }
        sfxAudioSource.Play();
    }

    private List<EnemyController> GetCloseEnemies()
    {   
        List<EnemyController> closeEnemies = new List<EnemyController>();

        for (int i = 0; i < _enemies.Length; ++i)
        {
            Vector3 offset = transform.position - _enemies[i].transform.position;
            float   sqrLen = offset.sqrMagnitude;

            if (sqrLen < _kickDistanceSqr)
                closeEnemies.Add(_enemies[i]);
        }

        return closeEnemies;
    }

    private List<Item> GetInFrontItems() {
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, 2.5f, transform.position.z), kickDistance);
        List<Item> closeItems = new List<Item>();
        for(int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders[i].tag == "item")
            {
              

                Vector3 directionToTarget = hitColliders[i].transform.position - transform.position;
                float angle = Vector3.Angle(Quaternion.AngleAxis(-0.5f * sightAngle, transform.up) * transform.forward, directionToTarget);
                float distance = directionToTarget.magnitude;

                if(Mathf.Abs(angle) < sightAngle && distance > transform.localScale.x)
                {
                    closeItems.Add(hitColliders[i].GetComponent<Item>());
                }
            }
        }

        return closeItems;
    }

    private void OnDrawGizmos() 
    {
        UnityEditor.Handles.color = Color.yellow;

        UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, kickDistance);

        UnityEditor.Handles.color = Color.green;
        Vector3 fromVector = Quaternion.AngleAxis(-0.5f * sightAngle, transform.up) * transform.forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, fromVector, sightAngle,  kickDistance);
    }


	public void Hit (float dmg)
	{
		_hp -= dmg;

		if (_hp <= 0f)
		{
			isDead = true;
			dungeon.PlayerDead ();
		}
	}

	public void Resurect ()
	{
		isDead = false;
		_hp = fullHp;
	}
}