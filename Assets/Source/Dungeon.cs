﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour , IFrequency{

    [HideInInspector] public Camera playerCamera = null;
    [HideInInspector] public PlayerController playerController = null;
	public RoomSpawner[]	roomSpawners = null;
	public PlayerSpawner playerSpawn	= null;
	public int id  = 0;

	[SerializeField]
	private Frequency _frequency;
	public Frequency frequency { get { return _frequency; } set { _frequency = value; } }

	public Game game;
	public Renderer[] renderers;

	public void Start ()
	{
		for (int i = 0; i < renderers.Length; i++)
		{
			//Send freqency
		}

        ColorBehaviour[] colorBehaviours = GetComponentsInChildren<ColorBehaviour>(true);
        for (int i = 0; i < colorBehaviours.Length; i++)
            colorBehaviours[i].SetFrequency(_frequency);
    }

	// Use this for initialization
	public void StartDungeon (PlayerController playerPrefab, Camera playerCamera) 
	{
        playerController = GameObject.Instantiate<PlayerController> (playerPrefab, playerSpawn.transform.position, Quaternion.identity);

		playerController.playerId = id;
        playerController.gameObject.name = "Player #" + id;
		playerController.frequency = frequency;
		playerController.dungeon = this;
        playerController.playerCamera = playerCamera;

		for (int i = 0; i < roomSpawners.Length; i++)
		{
			roomSpawners [i].SetPlayer (playerController);
			roomSpawners [i].frequency = frequency;
            roomSpawners[i].StartRoom();
		}
        game.registerPlayer(playerController);
	}


	public void PlayerDead ()
	{
		Debug.Log ("player " + playerController.playerId + " is dead at " + Time.realtimeSinceStartup);
		game.PlayerDead (playerController);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
