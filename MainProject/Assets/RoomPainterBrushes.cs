using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public static class RoomPainterBrushes
{
    public static void RoomRectangle()
    {

    }
    public static void SetTile(Level level, int x, int y, string tile)
    {
        //Debug.Log(x + " " + y + " " + level);
        level.grid[x, y].walkable = GetTile(tile).walkable;
        level.grid[x, y].type = GetTile(tile).type;
    }
    public static void LevelRectangle(Level level, int fromX, int fromY, int toX, int toY, string tile)
    {
        
        for (int i = fromX; i <= toX; i++)
        {
            for (int j = fromY; j <= toY; j++)
            {
                SetTile(level, i, j, tile);
            }
        }

    }
    public static void Line(Level level, int fromX, int fromY, int toX, int toY, string tile)
    {
        float x = fromX;
        float y = fromY;
        float dx = toX - fromX;
        float dy = toY - fromY;

        bool movingbyX = Math.Abs(dx) >= Math.Abs(dy);
        //normalize
        if (movingbyX)
        {
            dy /= Math.Abs(dx);
            dx /= Math.Abs(dx);
        }
        else
        {
            dx /= Math.Abs(dy);
            dy /= Math.Abs(dy);
        }

        SetTile(level, (int)Math.Round(x), (int)Math.Round(y), tile);

        while ((movingbyX && toX != x) || (!movingbyX && toY != y))
        {
            x += dx;
            y += dy;
            SetTile(level, (int)Math.Round(x), (int)Math.Round(y), tile);
        }
    }

    public static void Fill(Level level, int fromX, int fromY, string tile, int power = 2000)
    {
        List<GridNode> openList = new List<GridNode>();

        string tileType = level.grid[fromX, fromY].type;

        int counter = 0;

        openList.Add(level.grid[fromX, fromY]);
        while (openList.Count > 0 && counter < power)  {
            GridNode currentNode = openList[0];
            SetTile(level, currentNode.gridX, currentNode.gridY, tile);

            foreach (var neighbour in level.GetNeighbourds(currentNode)) {
                if (neighbour.type == tileType && !neighbour.checkedTrue) {
                    openList.Add(neighbour);
                    neighbour.checkedTrue = true;
                }
            }
            level.grid[currentNode.gridX, currentNode.gridY].checkedTrue = true;
            openList.Remove(currentNode);
            counter++;
        }
    }



    public static List<GridNode> TileTypes = new List<GridNode> {
        new GridNode(true, new Vector2Int(0,0),0,0, "FLOOR"),
        new GridNode(false, new Vector2Int(0,0),0,0, "WALL"),
        new GridNode(true, new Vector2Int(0,0),0,0, "DECO"),
        new GridNode(true, new Vector2Int(0,0),0,0, "MUD"),
        new GridNode(true, new Vector2Int(0,0),0,0, "WOODFLOOR"),
        new GridNode(true, new Vector2Int(0,0),0,0, "GRASS"),
        new GridNode(true, new Vector2Int(0,0),0,0, "GRASSSPECIAL"),
    };
    public static GridNode GetTile(string name) {
        switch (name)
        {
            default:
                return TileTypes[0];
            case "FLOOR": 
                return TileTypes[0];
            case "WALL":
                return TileTypes[1];
            case "DECO":
                return TileTypes[2];
            case "MUD":
                return TileTypes[3];
            case "WOODFLOOR":
                return TileTypes[4];
            case "GRASS":
                return TileTypes[5];
            case "GRASSSPECIAL":
                return TileTypes[6];
        }
    }

}
