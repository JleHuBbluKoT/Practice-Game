using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class TetrisGenerator
{
  
    public List<Room> GenerateRooms(Level level, Vector2Int center, int roomCount = 25, int minW = 5, int maxW = 15, int minH = 5, int maxH = 15)
    {
        List<Room> settledRooms = new List<Room>();
        Vector2Int castleBL = new Vector2Int(level.sizeX / 4, level.sizeY / 4);
        Vector2Int castleTR = new Vector2Int(level.sizeX / 4 * 3, level.sizeY / 4 * 3);

        
        
        int rWidth = UnityEngine.Random.Range(minW, maxW);
        int rHeight = UnityEngine.Random.Range(minH, maxH);
        Room seedRoom = new BasicRoom(level, center.x - rWidth/2, center.y - rHeight/2, center.x + rWidth/2, center.y + rHeight/2);
        settledRooms.Add(seedRoom);

        
        int counterOut = 0;
        while (settledRooms.Count < roomCount && counterOut < 500)
        {
            counterOut = counterOut + 1;
            rWidth = UnityEngine.Random.Range(minW, maxW); 
            rHeight = UnityEngine.Random.Range(minH, maxH);
            int availableSpaceW = castleTR.x - rWidth;
            int availableSpaceH = castleTR.y - rHeight;

            int counter = 20;
            counter++;
            int pointX = UnityEngine.Random.Range(castleBL.x, availableSpaceW);
            int pointY = UnityEngine.Random.Range(castleBL.y, availableSpaceH);
            Room checkRoom = new BasicRoom(level, pointX, pointY, pointX + rWidth, pointY + rHeight);
            if (!listIntersectAlt(checkRoom, settledRooms)) {
                settledRooms.Add(checkRoom);
            }
        }
        
        /*
        Room fff = new BasicRoom(level, 74, 74, 76, 76);
        fff.ShiftRoom(10, 15);
        settledRooms.Add(fff);
        fff = new BasicRoom(level, 70, 70, 80, 80);
        settledRooms.Add(fff);*/
        /*
        fff = new BasicRoom(level, 68, 82, 82, 100);
        settledRooms.Add(fff);*/

        List<Room> sortedList = settledRooms.OrderBy(x => distanceBetweenPoints(center, x.center())).ToList();
        for (int i = 0; i < sortedList.Count; i++){
            sortedList[i].indexRL = i;
        }
        for (int i = 0; i < sortedList.Count; i++) {
            SoftShiftNumbers(center, sortedList[i], sortedList);
        }

        for (int i = 0; i < sortedList.Count; i++)
        {
            sortedList[i].Resize(1,1);
        }

        //Graph.ConnectionLogic(sortedList, level);


        return sortedList;
    }

    public bool listIntersect(Room room, List<Room> roomList) {
        foreach (var item in roomList) {
            if (room.indexRL != item.indexRL) {
                if (room.CheckIntersect(item)) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool listIntersectAlt(Room room, List<Room> roomList) {
        foreach (var item in roomList) { 
            if (room.CheckIntersect(item)) {
                return true;
            }
        }
        return false;
    }

    public void SoftShiftNumbers(Vector2Int center, Room room, List<Room> roomList)
    {
        Vector2Int roomCenter = room.center();
        float x = roomCenter.x;
        float y = roomCenter.y;
        float dx = center.x - roomCenter.x;
        float dy = center.y - roomCenter.y;

        bool movingbyX = Math.Abs(dx) >= Math.Abs(dy);
        //normalize
        if (movingbyX) {
            dy /= Math.Abs(dx);
            dx /= Math.Abs(dx);
        }
        else {
            dx /= Math.Abs(dy);
            dy /= Math.Abs(dy);
        }
        
        float counter = 0;
        while (((movingbyX && center.x != x) || (!movingbyX && center.y != y)) && counter < 40)
        {
            if (counter > 40) {
                break;
            }
            counter = counter + 1;
            if (!listIntersect(room, roomList)) {
                Room prevPos = room.ShiftRoomGet(0,0);
                x += dx;
                y += dy;
                Vector2 fCenter = new Vector2(x, y);
                //room.ShiftRoom(-(int)Math.Round(room.center().x - x), -(int)Math.Round(room.center().y - y));

                Room futureRoomX = room.ShiftRoomGet(-Math.Sign(room.center().x - x), 0);
                //Debug.Log(room.center() + " " + futureRoomX.center());
                //Debug.Log(-(int)Math.Round(room.center().x - x));
                //Debug.Log(!listIntersect(futureRoomX, roomList));
                if (!listIntersect(futureRoomX, roomList) ) { 
                    room.ShiftRoom(-(int)Math.Sign(room.center().x - x), 0);
                }


                Room futureRoomY = room.ShiftRoomGet(0, -Math.Sign(room.center().y - y));
                /*Debug.Log(room.center() + " " + futureRoomY.center());
                Debug.Log(-(int)Math.Round(room.center().y - y));
                Debug.Log(!listIntersect(futureRoomY, roomList));*/
                if (!listIntersect(futureRoomY, roomList)) {
                    room.ShiftRoom(0, -(int)Math.Sign(room.center().y - y));
                }
                //RoomPainterBrushes.SetTile(room.grid, room.center().x, room.center().y, "DECO");
                if (prevPos.center() == room.center()) {
                    counter++;
                }
            }
        }
    }




    public float angleBetweenPoints(Vector2 from, Vector2 to, bool allowNegatives = false)  {
        float angle = (float)(180 / Math.PI * (Math.Atan2(to.y - from.y, to.x - from.x) + Math.PI / 2));
        if (angle < 0 && !allowNegatives) { angle = angle + 360; }
        return angle;
    }

    public List<Vector2Int> GetSpawnPoints(Vector2Int center, int distance, int count) {
        List<Vector2Int> points = new List<Vector2Int>();
        for (int i = 0; i < count; i++) {
            float pAngle = UnityEngine.Random.Range(0, 360);
            Vector2 nextPoint = RoomCoordGen(center, pAngle, (int)Math.Round( (decimal) distance));
            points.Add( new Vector2Int( (int)Math.Round( nextPoint.x), (int)Math.Round(nextPoint.y)));
        }
        return points;
    }

    public Vector2 RoomCoordGen(Vector2 center, float coreAngle, int distance) {
        float value1 = center.x + (float)(distance * Math.Cos(PointAngle(coreAngle)));
        float value2 = center.y + (float)(distance * Math.Sin(PointAngle(coreAngle)));
        Vector2 roomCoords = new Vector2(value1, value2);
        return roomCoords;
    }

    public float PointAngle(float angle) {
        return (float)(Math.PI / 180 * (angle % 360));
    }
    public float distanceBetweenPoints(Vector2 from, Vector2 to)
    {
        return (float)Math.Sqrt((to.x - from.x) * (to.x - from.x) + (to.y - from.y) * (to.y - from.y));
    }

}
/*
while (((movingbyX && center.x != x) || (!movingbyX && center.y != y)) && counter < 1)
{
    if (!listIntersect(room, roomList))
    {
        Room prevPos = room.ShiftRoomGet(0, 0);
        x += dx;
        y += dy;
        Vector2 fCenter = new Vector2(x, y);
        //room.ShiftRoom(-(int)Math.Round(room.center().x - x), -(int)Math.Round(room.center().y - y));

        Room futureRoomX = room.ShiftRoomGet(-(int)Math.Round(room.center().x - x), 0);
        Debug.Log(room.center() + " " + futureRoomX.center());
        Debug.Log(-(int)Math.Round(room.center().x - x));
        Debug.Log(!listIntersect(futureRoomX, roomList));
        if (!listIntersect(futureRoomX, roomList))
        {
            room.ShiftRoom(-(int)Math.Round(room.center().x - x), 0);
        }


        Room futureRoomY = room.ShiftRoomGet(0, -(int)Math.Round(room.center().y - y));
        Debug.Log(room.center() + " " + futureRoomY.center());
        Debug.Log(-(int)Math.Round(room.center().y - y));
        Debug.Log(!listIntersect(futureRoomY, roomList));
        if (!listIntersect(futureRoomY, roomList))
        {
            room.ShiftRoom(0, -(int)Math.Round(room.center().y - y));
        }

        RoomPainterBrushes.SetTile(room.grid, room.center().x, room.center().y, "DECO");

        if (prevPos.center() == room.center())
        {
            Debug.Log("stuck");
            counter++;
        }
    }
}*/
