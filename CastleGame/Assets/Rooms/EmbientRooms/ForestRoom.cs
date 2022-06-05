using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestRoom : Room
{
    public ForestRoom(Level _grid, int _left, int _bottom, int _right, int _top)
    {
        grid = _grid;
        bottom = _bottom;
        left = _left;
        top = _top;
        right = _right;
    }
    public override void GetExits()
    {

    }
    public override void GetTiles()
    {
        RoomPainterBrushes.LevelRectangle(grid, left + 1, bottom + 1, right - 1, top - 1, "MUD");
        float fullness = 0.7f;
        for (int i = left; i <= right; i++)
        {
            for (int j = bottom; j <= top; j++)
            {
                if (UnityEngine.Random.Range(0f, 1f) < fullness)
                {
                    RoomPainterBrushes.SetTile(grid, i, j, "GRASSSPECIAL");

                }
            }
        }
    }

    public override GridNode QuickFreeSpot()
    {
        return grid.grid[center().x, center().y];
    }
}
