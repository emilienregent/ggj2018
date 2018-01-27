using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public float moveSpeed = 15.0f;
    public float triggerDeadZone = 0.5f;
    
    public int playerId;
    public  float                strengh = 10f;   
    public  float                kickDistance = 2f;
    private float                _kickDistanceSqr = 0f;
    private EnemyController[]    _enemies = null;

    public GunController gun;
    public Camera playerCamera;
    private Vector3 cameraOffset;

    // Use this for initialization

    private void Start () {
        _kickDistanceSqr = kickDistance * kickDistance;

        // TOOO : Remove as soon as we have an enemy spawner
        _enemies = FindObjectsOfType<EnemyController>() as EnemyController[];

        // Get the camera offset
        cameraOffset = playerCamera.transform.position - transform.position;
	}

    // Update is called once per frame
    private void Update () {
        Move();
        Rotate();
        
        gun.isFiring = (Input.GetAxis("Player_" + playerId + "_Fire1") >= triggerDeadZone);

        if(Input.GetButtonDown("Player_" + playerId + "_Fire2"))
        {
            Debug.Log("SPELL 2");
        }

        if(Input.GetButtonDown("Player_" + playerId + "_Fire3"))
        {
            Kick();
        }

    }

    private void LateUpdate()
    {
        playerCamera.transform.position = transform.position + cameraOffset;
    }

    // get input from the left stick for player movement
    private void Move() {
        Vector3 heading = new Vector3(Input.GetAxis("Player_" + playerId + "_Horizontal") * moveSpeed * Time.deltaTime, 0, Input.GetAxis("Player_" + playerId + "_Vertical") * moveSpeed * Time.deltaTime);
        if(heading != Vector3.zero)
        {
            float distance = heading.magnitude;
            transform.position += heading / distance * moveSpeed * Time.deltaTime;
        }
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