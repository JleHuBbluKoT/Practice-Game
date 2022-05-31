using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Graph
{
    //public TextMesh prefab;
    public static void ConnectionLogic(List<Room> roomList, Level level, bool neighReset = false)
    {
        if (neighReset) {
            foreach (var room in roomList) {
                room.connectedRooms.Clear();            
            }
        }

        List<Room> checkedRooms = new List<Room> ();

        for (int i = 0; i < roomList.Count - 1; i++)
        {
            HashSet<int> neighbours = new HashSet<int>();



            for (int j = 1 + i; j < roomList.Count; j++) {
                if (i != j) {

                    //Debug.Log(checkedRooms.Count);
                    //Debug.Log( (roomList[i].intersect(roomList[j]).Area() > 3 ) + " " +  !roomList[i].connectedRooms.Contains(roomList[j]) + " " + !checkedRooms.Contains(roomList[i]));
                    if (roomList[i].CheckIntersect(roomList[j])) {
                        if (roomList[i].intersect(roomList[j]).Area() >= 5 && !roomList[i].connectedRooms.Contains(roomList[j]) && !checkedRooms.Contains(roomList[i])) {
                            neighbours.Add(roomList[j].indexRL);
                        }
                    }
                }
            }
            //Debug.Log("Lipoma!!!!!!!!!!!!!!!!!!!!!");
            foreach (var neighbour in neighbours)  {
                roomList[i].connectedRooms.Add(level.levelRoomList[neighbour]);
                
                if (!level.levelRoomList[neighbour].connectedRooms.Contains(roomList[i])) {
                    level.levelRoomList[neighbour].connectedRooms.Add(roomList[i]);
                }
            }
            
            

            checkedRooms.Add(roomList[i]);
        }
        /*
        foreach (var room in checkedRooms)
        {
            string q = " ";
            foreach (var neighbour in room.connectedRooms)
            {
                q = q + " " + neighbour.indexRL;

            }
            Debug.Log(room.indexRL + "_____" + q);
        }*/

        /*
        List<Room> usedRoom = new List<Room>();
        
        for (int i = 0; i < roomList.Count; i++)
        {
            foreach (var neighbour in roomList[i].connectedRooms)
            {
                
                if (!usedRoom.Contains(neighbour))
                {
                    RoomPainterBrushes.Line(level, neighbour.center().x, neighbour.center().y, roomList[i].center().x, roomList[i].center().y, "DECO");
                } 
            }
            usedRoom.Add(roomList[i]);
        }*/
        
    }

    public static void GraphRoomImport(List<Room> roomList, Level level)
    {

    }

    public static void CrearGraph(List<Room> roomList)
    {
        List<Room> checkedRooms = new List<Room>();
        for (int i = 0; i < checkedRooms.Count; i++) {
            checkedRooms[i].connectedRooms.Clear();
        }
    }

}
