using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DirectionEnum
{
    NONE,
    UP,
    RIGHT,
    DOWN,
    LEFT
}

public class Game : MonoBehaviour 
{
    private const float LINE_SIZE = 1f;
    private const float UP_DOWN_PERSPECTIVE_OFFSET = 5f;
    private const float LATERAL_PERSPECTIVE_OFFSET = 1.5f;

    private readonly Dictionary<int, DungeonConfig> DUNGEONS_CONFIG = new Dictionary<int, DungeonConfig>()
    {
        { 1, new DungeonConfig(3, 2, new List<DirectionEnum>() {DirectionEnum.RIGHT, DirectionEnum.DOWN})},
        { 2, new DungeonConfig(4, 1, new List<DirectionEnum>() {DirectionEnum.LEFT, DirectionEnum.DOWN})},
        { 3, new DungeonConfig(1, 4, new List<DirectionEnum>() {DirectionEnum.RIGHT, DirectionEnum.UP})},
        { 4, new DungeonConfig(2, 3, new List<DirectionEnum>() {DirectionEnum.LEFT, DirectionEnum.UP})}
    };

	public float fullPoolHp = 0f;
    public int LifePool = 6;
    public Canvas canvas = null;
    public GameObject warpFx = null;
	public PlayerController[] players = null;
	public Dungeon[] dungeons = null;
    public Camera[] cameras = null;
    public UI_Control ui_control;


    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsTrue(cameras.Length <= dungeons.Length, "Not enough camera for all " + dungeons.Length + " dungeons");
        UnityEngine.Assertions.Assert.IsTrue(players.Length <= dungeons.Length, "Not enough player for all " + dungeons.Length + " dungeons");
    }

	public float resurectDelay = 0f;

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < dungeons.Length; i++)
		{
            #if !NO_CONTROLLER            
            if(PlayerPrefs.HasKey("Player_" + (i + 1) + "_controller") == true)
            #endif
            {
                dungeons[i].game = this;
                dungeons[i].playerCamera = cameras[i];

                dungeons[i].StartDungeon (players [i], cameras[i]);
            }
            #if !NO_CONTROLLER
            else
            {
                dungeons[i].gameObject.SetActive(false);
                // cameras[i].gameObject.SetActive(false);
            }
            #endif
        }
	}

	public void PlayerDead (PlayerController player)
	{
        LifePool -= 1;
        //Debug.Log(LifePool);
        //fullPoolHp -= player.fullHp;

		if (LifePool > 0f)
		{
            ui_control.LoseLife(LifePool);
            player.Resurect (resurectDelay);
		}
		else
        {
            UnityEngine.Debug.Log("Game Over");


            int killCounter = 0;    int killPlayerId = 0;
            int kickCounter = 0;    int kickPlayerId = 0;
            int shotCounter = 0;    int shotPlayerId = 0;
            float hitCounter = 0;   int hitPlayerId = 0;
            float healCounter = 0;  int healPlayerId = 0;

            for (int i = 0; i < dungeons.Length; ++i)
            {
                if (dungeons[i].playerController != null)
                {
                    if (dungeons[i].playerController.GetKillCounter() > killCounter)
                    {
                        killCounter = dungeons[i].playerController.GetKillCounter();
                        killPlayerId = dungeons[i].playerController.playerId;
                    }

                    if (dungeons[i].playerController.GetKickCounter() > kickCounter)
                    {
                        kickCounter = dungeons[i].playerController.GetKickCounter();
                        kickPlayerId = dungeons[i].playerController.playerId;
                    }

                    if (dungeons[i].playerController.GetShotCounter() > shotCounter)
                    {
                        shotCounter = dungeons[i].playerController.GetShotCounter();
                        shotPlayerId = dungeons[i].playerController.playerId;
                    }

                    if (dungeons[i].playerController.GetHealCounter() > healCounter)
                    {
                        healCounter = dungeons[i].playerController.GetHealCounter();
                        healPlayerId = dungeons[i].playerController.playerId;
                    }

                    if (dungeons[i].playerController.GetHitCounter() > hitCounter)
                    {
                        hitCounter = dungeons[i].playerController.GetHitCounter();
                        hitPlayerId = dungeons[i].playerController.playerId;
                    }
                }
            }

            PlayerPrefs.SetInt("killPlayerId", killPlayerId);
            PlayerPrefs.SetInt("kickPlayerId", kickPlayerId);
            PlayerPrefs.SetInt("shotPlayerId", shotPlayerId);
            PlayerPrefs.SetInt("healPlayerId", healPlayerId);
            PlayerPrefs.SetInt("hitPlayerId", hitPlayerId);

            PlayerPrefs.SetInt("killCounter", killCounter);
            PlayerPrefs.SetInt("kickCounter", kickCounter);
            PlayerPrefs.SetInt("shotCounter", shotCounter);
            PlayerPrefs.SetInt("healCounter", (int)healCounter);
            PlayerPrefs.SetInt("hitCounter", (int)hitCounter);

            // Go back to the menu scene
            // TODO : Doing something better. Done.
            SceneManager.LoadScene(4);
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    public bool IsAvailableDirection(int currentDungeonId, DirectionEnum direction)
    {
        if (direction != DirectionEnum.NONE && DUNGEONS_CONFIG[currentDungeonId].availableDirections.Contains(direction) == false)
            return false;

        int dungeonId = GetDungeonId(currentDungeonId, direction);
        int dungeonIndex = dungeonId - 1;

        return dungeonIndex < dungeons.Length && dungeons[dungeonIndex].playerController != null;
    }

    public DirectionEnum GetDirection(int currentDungeonId, Vector3 position)
    {
        Vector3 viewPos = cameras[currentDungeonId - 1].WorldToViewportPoint(position);

        if (viewPos.x < 0)
            return DirectionEnum.LEFT;
        else if(viewPos.x > 1)
            return DirectionEnum.RIGHT;
        else if(viewPos.y < 0)
            return DirectionEnum.DOWN;
        else if(viewPos.y > 1)
            return DirectionEnum.UP;

        return DirectionEnum.NONE;
    }

    internal static void incrementKillCounter()
    {
        throw new NotImplementedException();
    }

    public void registerPlayer(PlayerController Player)
    {
        ui_control.registerPlayer(Player);
    }


    public int GetDungeonId(int currentDungeonId, DirectionEnum direction)
    {
        if(direction == DirectionEnum.LEFT || direction == DirectionEnum.RIGHT)
        {
            return DUNGEONS_CONFIG[currentDungeonId].horizontalNeighbour;
        }
        else if (direction == DirectionEnum.DOWN || direction == DirectionEnum.UP)
        {
            return DUNGEONS_CONFIG[currentDungeonId].verticalNeighbour;
        }

        return currentDungeonId;
    }

    public Vector3 GetPosition(Vector3 currentPosition, DirectionEnum direction, int currentDungeonId, int nextDungeonId)
    {
        /*Vector2 currentViewPos = _cameras[currentRoomId - 1].WorldToViewportPoint(currentPosition);

        UnityEngine.Debug.Log("Screen space pos out : " + currentViewPos + " ("+_cameras[currentRoomId - 1].ScreenToWorldPoint(new Vector3(currentViewPos.x, currentViewPos.y, _cameras[currentRoomId - 1].nearClipPlane))+")");

        Vector2 nextViewPos = 
            direction == DirectionEnum.UP ? new Vector2(currentViewPos.x, 0f) : 
            direction == DirectionEnum.DOWN ? new Vector2(currentViewPos.x, 1f) : 
            direction == DirectionEnum.RIGHT ? new Vector2(0f, currentViewPos.y) : 
            new Vector2(1f, currentViewPos.y);

        UnityEngine.Debug.Log("Screen space pos in : " + nextViewPos);

        Vector3 newPosition = _cameras[nextRoomId - 1].ScreenToWorldPoint(new Vector3(nextViewPos.x, nextViewPos.y, _cameras[nextRoomId - 1].nearClipPlane));

        UnityEngine.Debug.Log("Change position from room #" + currentPosition + " to room #" + newPosition);*/

        Vector3 currentOffset = currentPosition - dungeons[currentDungeonId - 1].playerController.transform.position;
        Vector3 nextOffset = 
            direction == DirectionEnum.UP ? new Vector3(currentOffset.x, -currentOffset.y, -currentOffset.z + LINE_SIZE + UP_DOWN_PERSPECTIVE_OFFSET) : 
            direction == DirectionEnum.DOWN ? new Vector3(currentOffset.x, -currentOffset.y, -currentOffset.z + LINE_SIZE) : 
            direction == DirectionEnum.RIGHT ? new Vector3(-currentOffset.x + LINE_SIZE + LATERAL_PERSPECTIVE_OFFSET, currentOffset.y, currentOffset.z) : 
            new Vector3(-currentOffset.x + LINE_SIZE -LATERAL_PERSPECTIVE_OFFSET, currentOffset.y, currentOffset.z);

        return dungeons[nextDungeonId - 1].playerController.transform.position + nextOffset;
    }

    public PlayerController GetPlayer(int dungeonId)
    {
        return dungeons[dungeonId - 1].playerController;
    }

    public void PlayWarpFx(Vector3 position, DirectionEnum direction, int currentDungeonId, int nextDungeonId)
    {
        Vector2 viewPos = cameras[currentDungeonId - 1].WorldToViewportPoint(position);

        GameObject entryFx = GameObject.Instantiate<GameObject>(warpFx, viewPos, Quaternion.identity);

        float halfWidth = (Screen.width * 0.5f);
        float halfHeight = (Screen.height * 0.5f);

        float x = direction == DirectionEnum.LEFT || direction == DirectionEnum.RIGHT ? 0f :
            currentDungeonId % 2 != 0 ? (viewPos.x - 1) * halfWidth : viewPos.x * halfWidth;

        float y = direction == DirectionEnum.UP || direction == DirectionEnum.DOWN ? 0f :
            currentDungeonId > 2 ? (viewPos.x - 1) * halfHeight : viewPos.x * halfHeight;

        float angle = 
            direction == DirectionEnum.LEFT ? -90f :
            direction == DirectionEnum.RIGHT ? 90f :
            direction == DirectionEnum.UP ? 180f :
            0f;
            

        entryFx.transform.position = new Vector2(x, y);
        entryFx.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        entryFx.transform.SetParent(canvas.transform, false);
    }
}