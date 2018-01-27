﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour 
{
	public float fullPoolHp = 0f;

	public PlayerController[] players = null;

	public Dungeon[] dungeons = null;

	public float resurectDelay = 0f;

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < dungeons.Length; i++)
		{
			dungeons [i].game = this;
			dungeons [i].StartDungeon (players [0]);
		}	
	}

	public void PlayerDead (PlayerController player)
	{
		fullPoolHp -= player.fullHp;

		if (fullPoolHp > 0f)
		{
			player.Resurect (resurectDelay);
		}
		else
			UnityEngine.Debug.Log ("Game Over");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
