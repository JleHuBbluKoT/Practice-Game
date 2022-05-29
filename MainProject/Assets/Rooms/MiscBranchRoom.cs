using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MiscBranchRoom : Room
{

    public MiscBranchRoom(Level _grid, int _left, int _bottom, int _right, int _top)
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
        RoomPainterBrushes.LevelRectangle(grid, left, bottom, right, top, "WALL");
        RoomPainterBrushes.LevelRectangle(grid, left + 1, bottom + 1, right - 1, top - 1, "FLOOR");

        int layers = (Math.Min(Width(), Height()) - 1) / 2;

        for (int i = 1; i <= layers; i++)
        {
            if (i % 2 == 0)
            {
                RoomPainterBrushes.LevelRectangle(grid, left + 1 + i, bottom + 1 + i, right - 1 - i, top - 1 - i, "FLOOR");
            } else
            {
                RoomPainterBrushes.LevelRectangle(grid, left + 1 + i, bottom + 1 + i, right - 1 - i, top - 1 - i, "WALL");
            }
            
        }
    }
}
