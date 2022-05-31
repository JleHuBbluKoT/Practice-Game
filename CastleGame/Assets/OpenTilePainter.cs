 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OpenTilePainter : MonoBehaviour
{
    
    [SerializeField]
    public static Tilemap roomTileMapF;
    [SerializeField]
    protected TileBase tile;

    public static void PaintTiles(Tilemap roomTileMap, TileBase tile, IEnumerable<Vector2Int> positions, Transform scale)
    {
        foreach (var position in positions)
        {
            var tilePosition = roomTileMap.WorldToCell(new Vector3(position.x * scale.transform.localScale.x, position.y * scale.transform.localScale.y, 0));
            roomTileMap.SetTile(tilePosition, tile);
            //roomTileMap.SetColor(tilePosition, new Color());
        }
    }

    public static void PaintSingleTile(Tilemap roomTileMap, TileBase tile, Vector2Int position, Transform scale)
    {

        var tilePosition = roomTileMap.WorldToCell(new Vector3( position.x * scale.transform.localScale.x, position.y * scale.transform.localScale.y, 0 ));
        roomTileMap.SetTile(tilePosition, tile);
    }

    public static void Clear(Tilemap roomTileMap)
    {
        roomTileMap.ClearAllTiles();
    }


}
