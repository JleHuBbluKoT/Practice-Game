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



    public int minWidth() { return -1; }
	public int maxWidth() { return -1; }

	public int minHeight() { return -1; }
	public int maxHeight() { return -1; }


}