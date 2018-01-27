using System;
using System.Collections.Generic;

public class DungeonConfig
{
    private int _verticalNeighbour     = 0;
    private int _horizontalNeighbour   = 0;
    private List<DirectionEnum> _availableDirections = null; 

    public int verticalNeighbour { get { return _verticalNeighbour; } }
    public int horizontalNeighbour { get { return _horizontalNeighbour; } }
    public List<DirectionEnum> availableDirections { get { return _availableDirections; } }

    public DungeonConfig(int verticalNeighbour, int horizontalNeighbour, List<DirectionEnum> availableDirections)
    {
        _verticalNeighbour = verticalNeighbour;
        _horizontalNeighbour = horizontalNeighbour;

        _availableDirections = availableDirections;
    }
}