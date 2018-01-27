using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour 
{
	public EnemyController[] spawns = null;
	public float spawnDate = 0f;

	private RoomSpawner _roomSpawner = null;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetRoomSpawner (RoomSpawner roomSpawner)
	{
		_roomSpawner = roomSpawner;
	}

	public void Spawn (PlayerController targetPlayer)
	{
		spawnDate = Time.realtimeSinceStartup;
		EnemyController enemyController = GameObject.Instantiate<EnemyController>(spawns [Random.Range (0, spawns.Length)], transform.position, Quaternion.identity);
		enemyController.SetPlayer (targetPlayer);
		enemyController.SetRoomSpawner (_roomSpawner);
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		Gizmos.DrawCube (transform.position, Vector3.one);
	}
}
