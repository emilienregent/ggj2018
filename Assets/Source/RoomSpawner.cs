using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour, IFrequency
{
	public Vector2				spawnDelay		= Vector2.zero;
	public MonsterSpawner[]		spawners		= null;
	public int 					monsterLimit	= 0;

	private float				_timeToSpawn 	= 0f;
	private int					_monsterCount	= 0;
	private PlayerController	_player			= null;
    private bool                _isStarted      = false;


	// Use this for initialization
	public Frequency frequency { get; set; }

    public void StartRoom ()
	{
		for (int i = 0; i < spawners.Length; i++)
		{
			spawners [i].SetRoomSpawner (this);
			spawners [i].frequency = frequency;
		}

        _isStarted = true;
	}

	public void SetPlayer(PlayerController player)
	{
		_player = player;
	}

	public void StartSpawner () 
	{
		SetTimeToSpawn ();
	}

	void SetTimeToSpawn ()
	{
		_timeToSpawn = Time.realtimeSinceStartup + Random.Range (spawnDelay.x, spawnDelay.y);
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (_isStarted == false)
            return;
        
		if (Time.realtimeSinceStartup >= _timeToSpawn)
		{
			SetTimeToSpawn ();

			if (_monsterCount < monsterLimit)
			{
				spawners [Random.Range (0, spawners.Length)].Spawn (_player);
				_monsterCount++;
			}
		}
	}

	public void MonsterKill(EnemyController enemyController)
	{
		_monsterCount--;
	}
}
