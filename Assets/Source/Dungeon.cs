using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour {

	public PlayerController playerController = null;
	public RoomSpawner[]	roomSpawners = null;

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < roomSpawners.Length; i++)
			roomSpawners [i].SetPlayer (playerController);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
