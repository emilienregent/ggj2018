using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour, IFrequency
{
    public ColorBehaviour colorBehaviour = null;
	public EnemyController[] spawns = null;
	public float spawnDate = 0f;

	private RoomSpawner _roomSpawner = null;
	public Frequency frequency { get; set; }

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetRoomSpawner (RoomSpawner roomSpawner, Frequency frequency)
	{
		_roomSpawner = roomSpawner;
        this.frequency = frequency;

        colorBehaviour.SetFrequency(frequency);
	}

	public void Spawn (PlayerController targetPlayer)
	{
		spawnDate = Time.realtimeSinceStartup;
		EnemyController enemyController = GameObject.Instantiate<EnemyController>(spawns [Random.Range (0, spawns.Length)], transform.position, Quaternion.identity);
		enemyController.SetPlayer (targetPlayer);
		enemyController.SetRoomSpawner (_roomSpawner);
		enemyController.frequency = frequency;
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		Gizmos.DrawCube (transform.position, Vector3.one);
	}
}
