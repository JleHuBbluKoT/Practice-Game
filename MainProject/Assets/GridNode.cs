using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public bool walkable;
    public string type;
    public Vector2Int worldPos;
    public int gridX;
    public int gridY;

    public bool checkedTrue = false;
    public int gCost;
    public int hCost;
    public int fCost { get { return gCost + hCost; } }

    public GridNode parent;

    public GridNode(bool _walkable, Vector2Int _worldPos, int _gridX, int _gridY, string _type = "FLOOR")
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        type = _type;
    }

}
