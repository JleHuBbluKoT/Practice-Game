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
    public GameObject associatedObject = null;
    public int fCost { get { return gCost + hCost; } }

    public GridNode parent;

    public GridNode(bool _walkable, Vector2Int _worldPos, int _gridX, int _gridY, string _type = "FLOOR") {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        type = _type;
    }

    public void setHueristics(Vector2Int target)
    {
        hCost = simpleDist(gridX, gridY, target);
    }

    public int simpleDist(int x, int y, Vector2Int target)
    {
        int dX = Mathf.Abs(x - target.x);
        int dY = Mathf.Abs(y - target.y);
        return Mathf.Min(dX, dY) * 4 + Mathf.Max(dX, dY) * 10;
    }
}
