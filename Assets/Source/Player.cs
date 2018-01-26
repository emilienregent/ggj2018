using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 position = transform.position;
            position.x--;
            transform.position = position;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 position = transform.position;
            position.x++;
            transform.position = position;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 position = transform.position;
            position.z++;
            transform.position = position;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 position = transform.position;
            position.z--;
            transform.position = position;
        }



    }
}
