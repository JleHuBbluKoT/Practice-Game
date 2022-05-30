using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileLib : MonoBehaviour
{
    [SerializeField]
    public Tilemap roomTileMapF, roomTileMapW;
    [SerializeField]
    public TileBase mud, wallTile, floorDecorative, grass, grassSpecial, woodFloor;

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
                    OpenTilePainter.PaintSingleTile(roomTileMapF, mud, node.worldPos); break;
                case "WOODFLOOR":
                    OpenTilePainter.PaintSingleTile(roomTileMapF, woodFloor, node.worldPos); break;
                case "DECO":
                    OpenTilePainter.PaintSingleTile(roomTileMapF, floorDecorative, node.worldPos); break;
                case "GRASS":
                    OpenTilePainter.PaintSingleTile(roomTileMapF, grass, node.worldPos); break;
                case "GRASSSPECIAL":
                    OpenTilePainter.PaintSingleTile(roomTileMapF, grassSpecial, node.worldPos); break;
                case "WALL":
                    OpenTilePainter.PaintSingleTile(roomTileMapW, wallTile, node.worldPos); break;
                default:
                    OpenTilePainter.PaintSingleTile(roomTileMapF, mud, node.worldPos);
                break;
            }
        }

        OpenTilePainter.PaintTiles(roomTileMapW, wallTile, queryW);


    }
    public void ClearAll()
    {
        OpenTilePainter.Clear(roomTileMapF);
        OpenTilePainter.Clear(roomTileMapW);
    }
}
