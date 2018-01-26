using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

    private Camera[] cams;
    public Camera currentCam;

	// Use this for initialization
	void Start ()
    {

        cams = Camera.allCameras;

    }
	
	// Update is called once per frame
	void Update () {

        //Vector3 viewPos = currentCam.WorldToViewportPoint(transform.position);

        if(Input.GetKeyDown(KeyCode.LeftArrow))
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

        Vector3 viewPos = currentCam.WorldToViewportPoint(transform.position);

    }
}
