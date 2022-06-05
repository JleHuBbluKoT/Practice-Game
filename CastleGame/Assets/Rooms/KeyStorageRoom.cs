using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyStorageRoom : Room
{
    public KeyStorageRoom(Level _grid, int _left, int _bottom, int _right, int _top)
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

        RoomPainterBrushes.LevelRectangle(grid, center().x - 1, center().y - 1, center().x + 1, center().y + 1, "GRASS");

        grid.objectLib.PlaceKeyStorage(new Vector2Int(center().x, center().y), grid);
        
    }

    public override GridNode QuickFreeSpot()
    {
        return freeSpot();
    }


}
