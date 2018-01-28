using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Potion : Item {

    public int healAmount;

    public override void ActiveItem(PlayerController player) {
        player.heal(healAmount);
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        _agent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);

        Move();
    }
}
