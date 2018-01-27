using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public float moveSpeed = 15.0f;

    private Rigidbody playerRigidbody;


    public  float                strengh = 10f;   
    public  float                kickDistance = 2f;
    private float                _kickDistanceSqr = 0f;
    private EnemyController[]    _enemies = null;

    public GunController gun;

    // Use this for initialization
    void Start () {

        _kickDistanceSqr = kickDistance * kickDistance;

        playerRigidbody = GetComponent<Rigidbody>();

        // TOOO : Remove as soon as we have an enemy spawner
        _enemies = FindObjectsOfType<EnemyController>() as EnemyController[];
	}
	
	// Update is called once per frame
	void Update () {
        move();
        rotate();
       
        if(Input.GetButtonDown("Fire1"))
        {
            gun.isFiring = true;
        }

        if(Input.GetButtonUp("Fire1"))
        {
            gun.isFiring = false;
        }

        if(Input.GetButtonDown("Fire2"))
        {
            Debug.Log("SPELL 2");
        }

        if(Input.GetButtonDown("Fire3"))
        {
            Kick();
        }

    }

    // get input from the left stick for player movement
    private void move() {
        
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVelocity = moveInput * moveSpeed;
        playerRigidbody.velocity = moveVelocity;
    }

    // get input from right stick for the player rotation
    private void rotate() {
        Vector3 direction = Vector3.right * Input.GetAxisRaw("RotationH") + Vector3.forward * -Input.GetAxisRaw("RotationV");
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

    private void OnDrawGizmos() 
    {
        UnityEditor.Handles.color = Color.yellow;

        UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, kickDistance);
    }
}