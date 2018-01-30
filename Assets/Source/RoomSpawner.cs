
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// L'important, c'est les valeurs.
/// </summary>
[System.Serializable]
public class ScaleValue
{
	[System.Serializable]
	public class Tupple
	{
		[SerializeField]
		public float cap		=	0f;

		[SerializeField]
		public float value		=	0f;
	}

	[SerializeField]
	public List<Tupple> scales = null;

	private int	_index = 0;

	public float cap { get { return scales [_index].cap; } }
	public float value { get { return scales [_index].value; } }

	public void Update(float check)
	{
		if (_index < (scales.Count - 1) && check.CompareTo (scales [_index + 1].cap) > 0)
		{
			_index++;
		}
	}
}

public class RoomSpawner : MonoBehaviour, IFrequency
{
	public Vector2				spawnDelay		= Vector2.zero;
	public MonsterSpawner[]		spawners		= null;
	public int 					monsterLimit	= 0;

	private float				_timeToSpawn 	= 0f;
	private int					_monsterCount	= 0;
	private PlayerController	_player			= null;
    private bool                _isStarted      = false;


	public PatrolWaypoint[]		patrolPath		= null;

    private int NeededKills = 60;
    public int CurrentKills = 0;

	[SerializeField]
	public ScaleValue monsterLimitOverTime = null;

	[SerializeField]
	public ScaleValue monsterLimitOverKill  = null;

	[SerializeField]
	public ScaleValue monsterSpawnOverTime  = null; 

    // Use this for initialization
    public Frequency frequency { get; set; }

    public void StartRoom ()
	{
		for (int i = 0; i < patrolPath.Length; i++)
		{
			patrolPath [i].index = i;
		}

		for (int i = 0; i < spawners.Length; i++)
		{
            spawners [i].SetRoomSpawner (this, frequency);
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

			monsterLimitOverKill.Update (CurrentKills);
			monsterLimitOverTime.Update(Time.realtimeSinceStartup);
			monsterSpawnOverTime.Update (Time.realtimeSinceStartup);

			int modMonsterLimit = monsterLimit + (int)monsterLimitOverKill.value + (int)monsterLimitOverTime.value;
			Debug.Log("monster limit : " + modMonsterLimit + " . spawn : "  + monsterSpawnOverTime.value + " . count " + _monsterCount);

			if (_monsterCount < modMonsterLimit)
			{
				StartCoroutine (SpawnerMonster (modMonsterLimit));
			}
			else
			{
				SetTimeToSpawn ();
			}
		}
	}

	private IEnumerator SpawnerMonster (int modMonsterLimit)
	{
		for (int i = 0; i < (int)monsterSpawnOverTime.value && _monsterCount < modMonsterLimit; i++)
		{ 
			spawners [UnityEngine.Random.Range (0, spawners.Length)].Spawn (_player);
			_monsterCount++;
			yield return null;
		}

		yield return null;

		SetTimeToSpawn ();
	}

	public PatrolWaypoint GetPatrolPoint(EnemyController enemy, int waypointIndex)
	{
		if (waypointIndex >= 0)
		{
			waypointIndex = (waypointIndex + 1) % patrolPath.Length;
		}
		else
		{
			int index = 0;
			float minDistance = float.MaxValue;

			for (int i = 0; i < patrolPath.Length; i++)
			{
				float distance = Vector3.Distance (enemy.transform.position, patrolPath [i].transform.position);

				if (distance < minDistance)
				{
					minDistance = distance;
					index = patrolPath [i].index;
				}
			}

			waypointIndex = index;
		}

        UnityEngine.Assertions.Assert.IsTrue(patrolPath.Length > waypointIndex, "Waypoint " + waypointIndex + " doesn't exist in patrol path for Room Spawner " + this);

		return patrolPath [waypointIndex];
	}

	public void MonsterKill(EnemyController enemyController)
	{
		_monsterCount--;
        CurrentKills++;

        Debug.Log(CurrentKills + "KILLS!!!");
            if(CurrentKills >= NeededKills)
        {
            SceneManager.LoadScene(3);

        }
    }
}
