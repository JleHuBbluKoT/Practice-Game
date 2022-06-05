using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FarmFieldsRoom : Room
{
    public FarmFieldsRoom(Level _grid, int _left, int _bottom, int _right, int _top)
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
        for (int i = bottom; i < top; i++)
        {
            if (i % 2 == 0)
            {
                RoomPainterBrushes.Line(grid, left + 1, i, right - 1, i, "GRASS");
            }
        }

    }

    public override GridNode QuickFreeSpot()
    {
        return grid.grid[center().x, center().y];
    }
}