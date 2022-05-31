using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PolygonRoom
{
    public Level grid;
    public List<Vector2Int> pointsList;

    public PolygonRoom(Level _grid, List<Vector2Int> _pointsList)
    {
        grid = _grid;
        pointsList = _pointsList;
    }

    public abstract void GetTiles();
}
