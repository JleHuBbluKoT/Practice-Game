using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Room : Rect
{
    public List<Room> connectedRooms = new List<Room>();
    public List<GridNode> exits;
    public string status;
    public Level grid;

    public Room parent;
    public bool hasChildren;
    public int generation;
    public int indexRL; //room list index
    public abstract void GetExits();
    public abstract void GetTiles();

    public Room ChangeType(Room room)
    {
        room.left = left;
        room.bottom = bottom;
        room.right = right;
        room.top = top;
        room.connectedRooms = connectedRooms;
        room.status = status;
        room.indexRL = indexRL;
        room.connectedRooms = connectedRooms;
        return room;
    }

    public Room SetSize(int _left, int _bottom, int _right, int _top)
    {
        bottom = _bottom;
        left = _left;
        top = _top;
        right = _right;
        return this;
    }

    public void ShiftRoom(int x, int y)
    {
        bottom += y;
        left += x;
        top += y;
        right +=x ;
    }

    public Room ShiftRoomGet(int x, int y)
    {
        Room f = new BasicRoom(grid, left + x, bottom + y, right + x, top + y);
        f.indexRL = indexRL;
        return f;
    }

    public int minWidth() { return -1; }
	public int maxWidth() { return -1; }

	public int minHeight() { return -1; }
	public int maxHeight() { return -1; }

    public void CutDoor(Room neighbour)
    {
        Rect interWalls = this.intersect(neighbour);
        Vector2Int center = interWalls.center();
        RoomPainterBrushes.SetTile(grid, center.x, center.y, "WOODFLOOR");
        if (interWalls.Width() == 1) {
            grid.objectLib.PlaceDoor(new Vector2Int(center.x, center.y), false, grid);
        }
        else {
            grid.objectLib.PlaceDoor(new Vector2Int(center.x, center.y), true, grid);
        }
    }

    public abstract GridNode QuickFreeSpot();

    public GridNode freeSpot()
    {
        List<GridNode> canAccess = new List<GridNode>();
        HashSet<GridNode> haveAccess = new HashSet<GridNode>();
        foreach (var neighbour in connectedRooms)
        {
            Rect interWalls = this.intersect(neighbour);
            Vector2Int center = interWalls.center();
            canAccess.Add(grid.grid[center.x, center.y]);
        }

        int danger = 0;
        while (canAccess.Count > 0 && danger < 200) {
            danger++;
            foreach (var neigh in grid.GetNeighbourds(canAccess[0])) {
                if (neigh.gridX > left && neigh.gridX < right)  {
                    if (neigh.gridY > bottom && neigh.gridY < top) {
                        if (neigh.walkable == true && !haveAccess.Contains(neigh)) {
                            canAccess.Add(neigh);
                        }
                    }
                }
            }
            haveAccess.Add(canAccess[0]);
            canAccess.Remove(canAccess[0]);
        }
        canAccess.Clear();
        canAccess.AddRange(haveAccess);

        Debug.Log(canAccess.Count);

        int rand = UnityEngine.Random.Range(0, canAccess.Count - 1);

        return canAccess[rand];
    }

}
