using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour 
{
	public float fullPoolHp = 0f;

	public PlayerController[] players = null;

	public Dungeon[] dungeons = null;

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < dungeons.Length; i++)
		{
			dungeons [i].game = this;
			dungeons [i].StartDungeon (players [0]);
		}	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
