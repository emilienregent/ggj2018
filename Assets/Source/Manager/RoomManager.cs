using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionEnum
{
    NONE,
    UP,
    RIGHT,
    DOWN,
    LEFT
}

public class RoomManager : MonoBehaviour
{
    private readonly Dictionary<int, Room> ROOMS = new Dictionary<int, Room>()
    {
        { 1, new Room(3, 2, new List<DirectionEnum>() {DirectionEnum.RIGHT, DirectionEnum.DOWN})},
        { 2, new Room(4, 1, new List<DirectionEnum>() {DirectionEnum.LEFT, DirectionEnum.DOWN})},
        { 3, new Room(1, 4, new List<DirectionEnum>() {DirectionEnum.RIGHT, DirectionEnum.UP})},
        { 4, new Room(2, 3, new List<DirectionEnum>() {DirectionEnum.LEFT, DirectionEnum.UP})}
    };

    private const float LINE_SIZE = 1f;

    [SerializeField] private Camera[] _cameras = null;
    [SerializeField] private PlayerController[] _players = null;

    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsTrue(_cameras.Length <= ROOMS.Count, "Not enough camera for all " + ROOMS.Count + " rooms");
        UnityEngine.Assertions.Assert.IsTrue(_players.Length <= ROOMS.Count, "Not enough player for all " + ROOMS.Count + " rooms");
    }

    public bool IsAvailableDirection(int currentRoomId, DirectionEnum direction)
    {
        return direction == DirectionEnum.NONE || ROOMS[currentRoomId].availableDirections.Contains(direction);
    }

    public DirectionEnum GetDirection(int currentRoomId, Vector3 position)
    {
        Vector3 viewPos = _cameras[currentRoomId - 1].WorldToViewportPoint(position);

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

    public int GetRoomId(int currentRoomId, DirectionEnum direction)
    {
        if(direction == DirectionEnum.LEFT || direction == DirectionEnum.RIGHT)
        {
            return ROOMS[currentRoomId].horizontalNeighbour;
        }
        else if (direction == DirectionEnum.DOWN || direction == DirectionEnum.UP)
        {
            return ROOMS[currentRoomId].verticalNeighbour;
        }

        return currentRoomId;
    }

    public Vector3 GetPosition(Vector3 currentPosition, DirectionEnum direction, int currentRoomId, int nextRoomId)
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

        Vector3 currentOffset = currentPosition - _players[currentRoomId - 1].transform.position;
        Vector3 nextOffset = 
            direction == DirectionEnum.UP ? new Vector3(currentOffset.x, -currentOffset.y, -currentOffset.z + LINE_SIZE + 5f) : 
            direction == DirectionEnum.DOWN ? new Vector3(currentOffset.x, -currentOffset.y, -currentOffset.z + LINE_SIZE) : 
            direction == DirectionEnum.RIGHT ? new Vector3(-currentOffset.x + LINE_SIZE + 1.5f, currentOffset.y, currentOffset.z) : 
            new Vector3(-currentOffset.x + LINE_SIZE -1.5f, currentOffset.y, currentOffset.z);

        return _players[nextRoomId - 1].transform.position + nextOffset;
    }

    public PlayerController GetPlayer(int currentRoomId)
    {
        return _players[currentRoomId - 1];
    }
}