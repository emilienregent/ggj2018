using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public float moveSpeed = 15.0f;

    private Rigidbody playerRigidbody;

    public GunController gun;
    public Camera playerCamera;

    // Use this for initialization
    void Start () {
        playerRigidbody = GetComponent<Rigidbody>();
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
            Debug.Log("SPELL 3");
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
}
