using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour 
{
    private const float LINE_SIZE = 1f;

    private readonly Dictionary<int, DungeonConfig> DUNGEONS_CONFIG = new Dictionary<int, DungeonConfig>()
    {
        { 1, new DungeonConfig(3, 2, new List<DirectionEnum>() {DirectionEnum.RIGHT, DirectionEnum.DOWN})},
        { 2, new DungeonConfig(4, 1, new List<DirectionEnum>() {DirectionEnum.LEFT, DirectionEnum.DOWN})},
        { 3, new DungeonConfig(1, 4, new List<DirectionEnum>() {DirectionEnum.RIGHT, DirectionEnum.UP})},
        { 4, new DungeonConfig(2, 3, new List<DirectionEnum>() {DirectionEnum.LEFT, DirectionEnum.UP})}
    };

	public float fullPoolHp = 0f;
	public PlayerController[] players = null;
	public Dungeon[] dungeons = null;
    public Camera[] cameras = null;

    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsTrue(cameras.Length <= dungeons.Length, "Not enough camera for all " + dungeons.Length + " dungeons");
        UnityEngine.Assertions.Assert.IsTrue(players.Length <= dungeons.Length, "Not enough player for all " + dungeons.Length + " dungeons");
    }

	// Use this for initialization
	void Start () 
	{
        //yield return new WaitForSeconds(1f);

		for (int i = 0; i < dungeons.Length; i++)
		{
            if(PlayerPrefs.HasKey("Player_" + (i + 1) + "_controller") == true)
            {

                dungeons[i].game = this;
                dungeons[i].playerCamera = cameras[i];

                dungeons[i].StartDungeon (players [i], cameras[i]);
            }
            else
            {
                dungeons[i].gameObject.SetActive(false);
                // cameras[i].gameObject.SetActive(false);
            }
        }	

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

    public bool IsAvailableDirection(int currentDungeonId, DirectionEnum direction)
    {
        return direction == DirectionEnum.NONE || DUNGEONS_CONFIG[currentDungeonId].availableDirections.Contains(direction);
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

        Vector3 currentOffset = currentPosition - players[currentDungeonId - 1].transform.position;
        Vector3 nextOffset = 
            direction == DirectionEnum.UP ? new Vector3(currentOffset.x, -currentOffset.y, -currentOffset.z + LINE_SIZE + 5f) : 
            direction == DirectionEnum.DOWN ? new Vector3(currentOffset.x, -currentOffset.y, -currentOffset.z + LINE_SIZE) : 
            direction == DirectionEnum.RIGHT ? new Vector3(-currentOffset.x + LINE_SIZE + 1.5f, currentOffset.y, currentOffset.z) : 
            new Vector3(-currentOffset.x + LINE_SIZE -1.5f, currentOffset.y, currentOffset.z);

        return players[nextDungeonId - 1].transform.position + nextOffset;
    }

    public PlayerController GetPlayer(int currentDungeonId)
    {
        return dungeons[currentDungeonId - 1].playerController;
    }
}
