using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolWaypoint : MonoBehaviour {

	public int index = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = new Color(1f, 0f, 1f, 0.5f);
		Gizmos.DrawCube (transform.position, Vector3.one);
	}
}
