using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		playerController.frequency = frequency;
		playerController.dungeon = this;
        playerController.playerCamera = playerCamera;

		for (int i = 0; i < roomSpawners.Length; i++)
		{
			roomSpawners [i].SetPlayer (playerController);
			roomSpawners [i].frequency = frequency;
            roomSpawners[i].StartRoom();
		}
	}


	public void PlayerDead ()
	{
		Debug.Log ("player " + playerController.playerId + " is dead");

		game.fullPoolHp -= playerController.fullHp;

		playerController.Resurect ();

		if (game.fullPoolHp <= 0f)
		{
			Debug.Log ("Game Over");
            // Go back to the menu scene
            // TODO : Doing something better
            SceneManager.LoadScene(0);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
