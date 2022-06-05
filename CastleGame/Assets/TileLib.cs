using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileLib : MonoBehaviour
{
    [SerializeField]
    public Grid globalGrid;
    [SerializeField]
    public Tilemap roomTileMapF, roomTileMapW;
    [SerializeField]
    public TileBase mud, wallTile, floorDecorative, grass, grassSpecial, woodFloor, black;

    public void PaintLevel(Level level)
    {
        OpenTilePainter.Clear(roomTileMapF);
        OpenTilePainter.Clear(roomTileMapW);
        HashSet<Vector2Int> query = new HashSet<Vector2Int>();
        HashSet<Vector2Int> queryW = new HashSet<Vector2Int>();
        foreach (var node in level.grid)
        {
            switch (node.type) {
                case "MUD":
                    OpenTilePainter.PaintSingleTile(roomTileMapF, mud, node.worldPos, globalGrid.transform); break;
                case "WOODFLOOR":
                    OpenTilePainter.PaintSingleTile(roomTileMapF, woodFloor, node.worldPos, globalGrid.transform); break;
                case "DECO":
                    OpenTilePainter.PaintSingleTile(roomTileMapF, floorDecorative, node.worldPos, globalGrid.transform); break;
                case "GRASS":
                    OpenTilePainter.PaintSingleTile(roomTileMapF, grass, node.worldPos, globalGrid.transform); break;
                case "GRASSSPECIAL":
                    OpenTilePainter.PaintSingleTile(roomTileMapF, grassSpecial, node.worldPos, globalGrid.transform); break;
                case "WALL":
                    OpenTilePainter.PaintSingleTile(roomTileMapW, wallTile, node.worldPos, globalGrid.transform); break;
                case "BLACK":
                    OpenTilePainter.PaintSingleTile(roomTileMapW, black, node.worldPos, globalGrid.transform); break;
                default:
                    OpenTilePainter.PaintSingleTile(roomTileMapF, mud, node.worldPos, globalGrid.transform);
                break;
            }
        }

        //OpenTilePainter.PaintTiles(roomTileMapW, wallTile, queryW);


    }
    public void ClearAll()
    {
        OpenTilePainter.Clear(roomTileMapF);
        OpenTilePainter.Clear(roomTileMapW);
    }
}
