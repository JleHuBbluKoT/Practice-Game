using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceRoom : Room
{

    public EntranceRoom(Level _grid, int _left, int _bottom, int _right, int _top)
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
        RoomPainterBrushes.LevelRectangle(grid, left + 1, bottom + 1, right - 1, top - 1, "WOODFLOOR");
        RoomPainterBrushes.Line(grid, center().x - 2, center().y - 2, center().x + 2, center().y + 2, "WALL");
        RoomPainterBrushes.Line(grid, center().x - 2, center().y + 2, center().x + 2, center().y - 2, "WALL");
    }

    public override GridNode QuickFreeSpot()
    {
        return grid.grid[center().x, center().y];
    }
}
