using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawner : MonoBehaviour, IFrequency
{
    public ColorBehaviour colorBehaviour = null;
    public ParticleSystem spawnFxPrefab = null;
	public EnemyController[] spawns = null;
	public float spawnDate = 0f;

	private RoomSpawner _roomSpawner = null;
    private ParticleSystem _spawnFx = null;
	public Frequency frequency { get; set; }

	public float pourcentToSpawnBadFrequency = 0f;

	private List<int> enemieFrequencies = new List<int>();

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	private void Update () {
        if(_spawnFx != null)
        {
            if(_spawnFx.IsAlive() == false)
            {
                Destroy(_spawnFx.gameObject);
            }
        }
	}

    public void SetRoomSpawner (RoomSpawner roomSpawner, Frequency frequency)
	{
		_roomSpawner = roomSpawner;
        this.frequency = frequency;

        colorBehaviour.SetFrequency(frequency);

		enemieFrequencies = new List<int> () {1, 2, 4, 8};
		enemieFrequencies.Remove ((int)frequency);
	}

	public void Spawn (PlayerController targetPlayer)
	{
		spawnDate = Time.realtimeSinceStartup;
		EnemyController enemyController = GameObject.Instantiate<EnemyController>(spawns [Random.Range (0, spawns.Length)], transform.position, Quaternion.identity);
		enemyController.SetPlayer (targetPlayer);
		enemyController.SetRoomSpawner (_roomSpawner);

		float rand = Random.Range (0f, 100f);

		if (rand < pourcentToSpawnBadFrequency)
		{
			enemyController.frequency = (Frequency) enemieFrequencies [ Random.Range( 0, enemieFrequencies.Count)];			
		}
		else
		{
			enemyController.frequency = frequency;
		}

		ColorBehaviour[] colors = enemyController.GetComponentsInChildren<ColorBehaviour> ();
		for (int i = 0; i < colors.Length; i++)
		{
			colors [i].SetFrequency (enemyController.frequency);
		}

        ParticleSystem _spawnFx = GameObject.Instantiate<ParticleSystem>(spawnFxPrefab, transform.position, Quaternion.identity);

        _spawnFx.Play();
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		Gizmos.DrawCube (transform.position, Vector3.one);
	}
}
