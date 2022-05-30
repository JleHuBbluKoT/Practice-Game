using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BinarySplit
{
    public List<Room> candidates = new List<Room>();
    Level level;
    int minWidth = 6;
    int minHeight = 6;
    int roadWidth = 10;
    public List<Room> roads = new List<Room>();
    Room mainRoad;

    public BinarySplit(Level _level)
    { level = _level; }

    public List<Room> BinarySpacePartitioning(Room seedRoom, bool needParents = false, bool road = false)
    {
        Room samuel = seedRoom;
        samuel.generation = 1;
        List<Room> checkedList = new List<Room>();
        List<Room> currentList = new List<Room>();
        List<Room> nextList = new List<Room>();
        currentList.Add(samuel);
        
        while(currentList.Count > 0) {
            foreach (var room in currentList) {
                //Debug.Log("_____" + room.left + " " + room.bottom + " " + room.right + " " + room.top);
                List<Room> babies = Slice(room, road);
                nextList.AddRange(babies);
                checkedList.Add(room);
                
            }
            currentList.Clear();
            currentList.AddRange(nextList);
            nextList.Clear();
        }

        List<Room> empty = new List<Room>();
        if (!needParents) {
            //Debug.Log(checkedList.Count);
            for (int i = 0; i < checkedList.Count; i++)
            {
                if (!checkedList[i].hasChildren)
                {
                    empty.Add(checkedList[i]);
                }
            }
            checkedList = empty;
            //Debug.Log(checkedList.Count);
        }
        checkedList.AddRange(roads);

        for (int i = 0; i < checkedList.Count; i++)
        {
            checkedList[i].indexRL = i;
            //Debug.Log(checkedList[i].indexRL);
        }
        return checkedList;
    }



    public List<Room> Slice(Room room, bool road)
    {
        List<Room> result = new List<Room>();
        room.hasChildren = false;
        if (room.generation < 8) {
            if (room.Width() == room.Height()) {
                if (UnityEngine.Random.Range(0, 2) == 1) {
                    result = SliceVertical(room, road);
                    return result;
                }
                else {
                    result = SliceHorizontal(room, road);
                    return result;
                }
            }
            if (room.Width() > room.Height() ) {
                result = SliceVertical(room, road);
                return result;
            }
            if (room.Width() < room.Height()) {
                result = SliceHorizontal(room, road);
                return result;
            }
            
        }
        return result;
    }

    public List<Room> SliceEven(Room room, int count, bool horiz)
    {
        List<Room> slices = new List<Room>();
        
        for (int i = 0; i < count ; i++)
        {
            if (!horiz)
            {
                Room f = new ForestRoom(room.grid, room.left + (int)Math.Round((double)(room.Width() -1 ) / count * i), room.bottom, room.left + (int)Math.Round((double)(room.Width() -1 ) / count * (i + 1)), room.top);
                slices.Add(f);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (horiz)
            {
                Room f = new FarmFieldsRoom(room.grid, room.left, room.bottom + (int)Math.Round((double) (room.Height() - 1) / count * i), room.right, room.bottom + (int)Math.Round((double) (room.Height() - 1) / count * (i + 1)));
                f.status = "ROAD";
                slices.Add(f);
            }
        }
        

        return slices;
    }

    public List<Room> SliceVertical(Room room, bool makeRoad)
    {
        List<Room> result = new List<Room>();
        int w1 = (int)Math.Ceiling(room.Width() * UnityEngine.Random.Range(0.33f, 0.66f));
        int w2 = room.Width() - w1;
        //Debug.Log(w1 + " " + w2);
        int babyRoadWidth = (roadWidth - room.generation * 2);

        if (w1 > minWidth && w2 > minWidth)
        {
            
            Room room1 = new BasicRoom(level, room.left, room.bottom, room.left + w1, room.top);
            Room room2 = new BasicRoom(level, room.left + w1, room.bottom, room.right, room.top);
            if (makeRoad)
            {
                if (w1 - babyRoadWidth > minWidth && w2 - babyRoadWidth > minWidth && room.generation < 4)
                {
                    room1 = new BasicRoom(level, room.left, room.bottom, room.left + w1 - babyRoadWidth / 2, room.top);
                    room2 = new BasicRoom(level, room.left + w1 + babyRoadWidth / 2, room.bottom, room.right, room.top);
                    Room road = new MiscBranchRoom(level, room1.right, room1.bottom, room2.left, room2.top);
                    if (room.generation == 1) { mainRoad = road; }
                    roads.AddRange(SliceEven(road, road.Height() / 10, true));
                }
                if (w1 - 3 > minWidth && w2 - 3 > minWidth && room.generation == 4 && mainRoad.CheckIntersect(new BasicRoom(level, room.left + w1 - 3, room.bottom, room.left + w1 + 3, room.top)))
                {
                    room1 = new BasicRoom(level, room.left, room.bottom, room.left + w1 - 3, room.top);
                    room2 = new BasicRoom(level, room.left + w1 + 3, room.bottom, room.right, room.top);
                    Room road = new MiscBranchRoom(level, room1.right, room1.bottom, room2.left, room2.top);
                    roads.AddRange(SliceEven(road, road.Height() / 10, true));

                }
            }
            else
            {
                room1 = new BasicRoom(level, room.left, room.bottom, room.left + w1, room.top);
                room2 = new BasicRoom(level, room.left + w1, room.bottom, room.right, room.top);
            }
            /*Debug.Log("_____" + room.left + " " + room.bottom + " " + room.right + " " + room.top + " Params: " + room.Width() + " " + room.Height());
            Debug.Log("_____" + room1.left + " " + room1.bottom + " " + room1.right + " " + room1.top + " Params: " + room1.Width() + " " + room1.Height());
            Debug.Log("_____" + room2.left + " " + room2.bottom + " " + room2.right + " " + room2.top + " Params: " + room2.Width() + " " + room2.Height());*/
            
            room.hasChildren = true;
            room1.generation = room.generation + 1;
            room2.generation = room.generation + 1;
            result.Add(room1); result.Add(room2);
        }
        return result;
    }
    public List<Room> SliceHorizontal(Room room, bool makeRoad)
    {
        List<Room> result = new List<Room>();
        int h1 = (int)Math.Ceiling(room.Height() * UnityEngine.Random.Range(0.4f, 0.6f));
        int h2 = room.Height() - h1;
        //Debug.Log(h1 + " " + h2);
        int babyRoadWidth = roadWidth - room.generation * 2;

        if (h1 > minHeight && h2 > minHeight)
        {
            Room room1 = new BasicRoom(level, room.left, room.bottom, room.right, room.bottom + h1);
            Room room2 = new BasicRoom(level, room.left, room.bottom + h1, room.right, room.top);
            if (makeRoad)
            {
                if (h1 - babyRoadWidth > minHeight && h2 - babyRoadWidth > minHeight && room.generation < 4)
                {
                    room1 = new BasicRoom(level, room.left, room.bottom, room.right, room.bottom + h1 - babyRoadWidth / 2);
                    room2 = new BasicRoom(level, room.left, room.bottom + h1 + babyRoadWidth / 2, room.right, room.top);
                    Room road = new MiscBranchRoom(level, room1.left, room1.top, room2.right, room2.bottom);
                    if (room.generation == 1) { mainRoad = road; }
                    roads.AddRange(SliceEven(road, road.Width() / 10, false));
                }
                if (h1 - 3 > minHeight && h2 - 3 > minHeight && room.generation == 4 && mainRoad.CheckIntersect(new BasicRoom(level, room.left, room.bottom + h1 - 3, room.right, room.bottom + h1 + 3)))
                {
                    room1 = new BasicRoom(level, room.left, room.bottom, room.right, room.bottom + h1 - 3);
                    room2 = new BasicRoom(level, room.left, room.bottom + h1 + 3, room.right, room.top);
                    Room road = new MiscBranchRoom(level, room1.left, room1.top, room2.right, room2.bottom);
                    roads.AddRange(SliceEven(road, road.Width() / 10, false));
                }
            } else
            {
                room1 = new BasicRoom(level, room.left, room.bottom, room.right, room.bottom + h1);
                room2 = new BasicRoom(level, room.left, room.bottom + h1, room.right, room.top);
            }
            /*Debug.Log("_____" + room.left + " " + room.bottom + " " + room.right + " " + room.top + " Params: " + room.Width() + " " + room.Height());
            Debug.Log("_____" + room1.left + " " + room1.bottom + " " + room1.right + " " + room1.top + " Params: " + room1.Width() + " " + room1.Height());
            Debug.Log("_____" + room2.left + " " + room2.bottom + " " + room2.right + " " + room2.top + " Params: " + room2.Width() + " " + room2.Height());*/

            room.hasChildren = true;
            room1.generation = room.generation + 1;
            room2.generation = room.generation + 1;
            result.Add(room1); result.Add(room2);
        }
        return result;
    }

}
