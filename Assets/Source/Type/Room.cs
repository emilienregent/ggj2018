using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    private int _verticalNeighbour     = 0;
    private int _horizontalNeighbour   = 0;
    private List<DirectionEnum> _availableDirections = null; 

    public int verticalNeighbour { get { return _verticalNeighbour; } }
    public int horizontalNeighbour { get { return _horizontalNeighbour; } }
    public List<DirectionEnum> availableDirections { get { return _availableDirections; } }

    public Room(int verticalNeighbour, int horizontalNeighbour, List<DirectionEnum> availableDirections)
    {
        _verticalNeighbour = verticalNeighbour;
        _horizontalNeighbour = horizontalNeighbour;

        _availableDirections = availableDirections;
    }
}