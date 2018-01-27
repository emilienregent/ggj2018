using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour {

	NavMeshAgent agent = null;



	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.A))
		{
			
			agent.Warp(GameObject.Find ("DestinationA").transform.position);
		}
		else if (Input.GetKeyDown (KeyCode.Z))
		{
			agent.Warp(GameObject.Find ("DestinationZ").transform.position);
		}
		else if (Input.GetKeyDown (KeyCode.E))
		{
			agent.Warp(GameObject.Find ("DestinationE").transform.position);
		}
		else if (Input.GetKeyDown (KeyCode.R))
		{
			agent.Warp(GameObject.Find ("DestinationR").transform.position);
		}
	}
}
