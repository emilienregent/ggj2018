using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item {

    public int healAmount;

    public override void ActiveItem(PlayerController player) {
        player.heal(healAmount);
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);

        Move();
    }
}
