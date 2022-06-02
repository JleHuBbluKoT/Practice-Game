using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DoorPropagation
{

    public static void PropagateDoors(List<Room> roomList, Level level)
    {
        List<Room> openList = new List<Room>();
        List<Room> closedList = new List<Room>();

        //openList.Add(roomList[0]);

        int counter = 0;
        for (int i = 0; i < roomList.Count; i++) {
            foreach (var neighbour in roomList[i].connectedRooms)
            {
                if (!closedList.Contains(neighbour) && roomList.Contains(neighbour))
                {
                    roomList[i].CutDoor(neighbour);
                    counter++;
                    //RoomPainterBrushes.Line(level, neighbour.center().x, neighbour.center().y, roomList[i].center().x, roomList[i].center().y, "DECO");
                }
            }
            closedList.Add(roomList[i]);
        }

        /*

        while (openList.Count > 0 && counter < 40)
        {
            counter++;

            foreach (var neighbour in openList[0].connectedRooms)
            {
                if (!closedList.Contains(neighbour.indexRL))
                {
                    openList[0].CutDoor(neighbour);
                    openList.Add(neighbour);
                }
            }
            closedList.Add(openList[0].indexRL);
            Debug.Log(openList[0].indexRL);
            openList.RemoveAt(0);
            
        }
        */
    }
}
