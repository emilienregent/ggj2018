using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public float moveSpeed = 15.0f;

    private Rigidbody playerRigidbody;

    public int playerId;

    public GunController gun;

    // Use this for initialization
    private void Start () {
        playerRigidbody = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    private void Update () {
        Move();
        Rotate();
       
        if(Input.GetButtonDown("Player_"+playerId+"_Fire1"))
        {
            gun.isFiring = true;
        }

        if(Input.GetButtonUp("Player_" + playerId + "_Fire1"))
        {
            gun.isFiring = false;
        }

        if(Input.GetButtonDown("Player_" + playerId + "_Fire2"))
        {
            Debug.Log("SPELL 2");
        }

        if(Input.GetButtonDown("Player_" + playerId + "_Fire3"))
        {
            Debug.Log("SPELL 3");
        }

    }

    // get input from the left stick for player movement
    private void Move() {
        
        Vector3 moveInput = new Vector3(Input.GetAxis("Player_" + playerId + "_Horizontal"), 0, Input.GetAxis("Player_" + playerId + "_Vertical"));
        Vector3 moveVelocity = moveInput * moveSpeed;
        playerRigidbody.velocity = moveVelocity;
    }

    // get input from right stick for the player rotation
    private void Rotate() {
        Vector3 direction = Vector3.right * Input.GetAxisRaw("Player_" + playerId + "_RotationH") + Vector3.forward * -Input.GetAxisRaw("Player_" + playerId + "_RotationV");
        if(direction.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}
