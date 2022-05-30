using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicDungeon
{
    /*
    Level level;
    public BasicDungeon(Level _level)
    {
        level = _level;
    }
    public void GenerateLoopLevel()
    {

        StartingRoom coreRoom = new StartingRoom(level, new Vector2Int(0, 0), 2, 2, "CORE");
        StartingRoom startingRoom = new StartingRoom(level, new Vector2Int(0, 0), 2, 2, "MISC");
        StartingRoom endRoom = new StartingRoom(level, new Vector2Int(0, 0), 2, 2, "MISC");
        //Room MiscRoom;
        //List<DungeonRoom> clockwiseRoom = new List<DungeonRoom>();
        List<DungeonRoom> counterClockRoom = new List<DungeonRoom>();

        coreRoom.coords = new Vector2Int(level.sizeX / 2, level.sizeY / 2);


        coreRoom.roomWidth = UnityEngine.Random.Range(12, 36);
        coreRoom.roomHeight = UnityEngine.Random.Range(12, 36);
        int coreAngle = UnityEngine.Random.Range(1, 360);

        startingRoom.roomWidth = UnityEngine.Random.Range(8, 16);
        startingRoom.roomHeight = UnityEngine.Random.Range(8, 16);

        startingRoom.coords = RoomCoordGen(coreRoom, startingRoom, coreAngle, 4);

        endRoom.roomWidth = UnityEngine.Random.Range(8, 16);
        endRoom.roomHeight = UnityEngine.Random.Range(8, 16);
        endRoom.coords = RoomCoordGen(coreRoom, endRoom, coreAngle + 180, 4);

        int counterClockCount = 4 + UnityEngine.Random.Range(0, 3);
        //int ClockwiseCount = 4 + UnityEngine.Random.Range(0, 3);

        for (int i = 1; i <= counterClockCount; i++)
        {
            StartingRoom miscRoom = new StartingRoom(level, Vector2Int.zero, 2, 2, "MISC");
            miscRoom.roomWidth = UnityEngine.Random.Range(8, 12);
            miscRoom.roomHeight = UnityEngine.Random.Range(8, 12);
            miscRoom.coords = RoomCoordGen(coreRoom, miscRoom, coreAngle + (i) * (180 / (counterClockCount + 1)), 0);

            counterClockRoom.Add(miscRoom);
            Debug.Log(miscRoom.coords + "" + miscRoom.roomWidth + " " + miscRoom.roomHeight);

        }

        level.levelRooms.Add(coreRoom);
        level.levelRooms.Add(startingRoom);
        level.levelRooms.Add(endRoom);
        level.levelRooms.AddRange(counterClockRoom);

        int safety = 0;
        while (RoomSpread(level.levelRooms) > 0 && safety < 100)
        {
            if (safety == 99)
            {
                Debug.Log("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            }
            safety++;
        }
        



    }
    public Vector2Int RoomCoordGen(DungeonRoom firstRoom, DungeonRoom secondRoom, int coreAngle, int preferedDistance)
    {
        int roomDistance = SafeDistance(firstRoom, secondRoom, preferedDistance);
        int value1 = firstRoom.coords.x + Convert.ToInt32(roomDistance * Math.Cos(RoomAngle(coreAngle)));
        int value2 = firstRoom.coords.y + Convert.ToInt32(roomDistance * Math.Sin(RoomAngle(coreAngle)));
        Vector2Int roomCoords = new Vector2Int(value1, value2);
        return roomCoords;
    }
    public int SafeDistance(DungeonRoom firstRoom, DungeonRoom secondRoom, int preferedDistance)
    {
        int biggestWidth = Math.Max(firstRoom.roomWidth, secondRoom.roomWidth);
        int biggestHeight = Math.Max(firstRoom.roomHeight, secondRoom.roomHeight);
        int safetyDistance = Convert.ToInt32(Math.Sqrt(biggestWidth * biggestWidth + biggestHeight * biggestHeight) + preferedDistance);
        return safetyDistance;
    }

    public float RoomAngle(int angle)
    {
        return (float)(Math.PI / 180 * (angle % 360));
    }


    public int RoomSpread(List<DungeonRoom> roomList)
    {
        int counter = 0;
        for (int i = 0; i < roomList.Count - 1; i++)
        {
            for (int j = i + 1; j < roomList.Count; j++)
            {
                if (Intersection(roomList[i], roomList[j]))
                {
                    counter++;
                    Spread(roomList[i], roomList[j]);
                    Debug.Log("Intersection! Intersection!");
                }
            }

        }
        Debug.Log(counter);
        return counter;
    }
    public bool Intersection(DungeonRoom RoomA, DungeonRoom RoomB)
    {
        if ((RoomA.coords.x < RoomB.secondPoint().x) && (RoomA.secondPoint().x > RoomB.coords.x) && (RoomA.coords.y < RoomB.secondPoint().y) && (RoomA.secondPoint().y > RoomB.coords.y))
        {
            return true;
        } else { 
            return false; 
        }  
    }

    public void Spread(DungeonRoom RoomA, DungeonRoom RoomB)
    {
        Debug.Log("Spreading time! RoomA: " + RoomA.coords + " " + RoomA.roomWidth + " " + RoomA.roomHeight);
        Debug.Log("Spreading time! RoomB: " + RoomB.coords + " " + RoomB.roomWidth + " " + RoomB.roomHeight);
        if (RoomA.coords.x < RoomB.coords.x)
        {
            if (RoomA.status != "CORE")
            {
                RoomA.coords.x = RoomA.coords.x - 1;
            }

            if (RoomB.status != "CORE")
            {
                RoomB.coords.x = RoomB.coords.x + 1;
            }
        }

        if (RoomA.coords.x > RoomB.coords.x)
        {
            if (RoomA.status != "CORE")
            {
                RoomA.coords.x = RoomA.coords.x + 1;
            }

            if (RoomB.status != "CORE")
            {
                RoomB.coords.x = RoomB.coords.x - 1;
            }
        }


        if (RoomA.coords.y < RoomB.coords.y)
        {
            if (RoomA.status != "CORE")
            {
                RoomA.coords.y = RoomA.coords.y - 1;
            }

            if (RoomB.status != "CORE")
            {
                RoomB.coords.y = RoomB.coords.y + 1;
            }
        }

        if (RoomA.coords.y > RoomB.coords.y)
        {
            if (RoomA.status != "CORE")
            {
                RoomA.coords.y = RoomA.coords.y + 1;
            }

            if (RoomB.status != "CORE")
            {
                RoomB.coords.y = RoomB.coords.y - 1;
            }
        }
        Debug.Log("Spreading time! RoomA: " + RoomA.coords + " " + RoomA.roomWidth + " " + RoomA.roomHeight);
        Debug.Log("Spreading time! RoomB: " + RoomB.coords + " " + RoomB.roomWidth + " " + RoomB.roomHeight);
    }
    */
}
