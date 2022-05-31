using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RoomPicker
{

	public List<Room> LoopLevel(Level level/*, Room start, Room end*/)
    {
		Vector2 center = new Vector2((int)(level.sizeX / 2), (int)(level.sizeY / 2));
		List<Room> pickedRooms = new List<Room>();
		Room start; Room end;
		

		List<Vector2Int> points = CircleLevel(level, 2, 0.5f);

		start = PickRoom(points[0], level);
		end = PickRoom(points[1], level);


		//Debug.Log(start.center() + " " + end.center());

		pickedRooms.AddRange(RoomGo(level, start, end));
		pickedRooms.AddRange(RoomGo(level, end, start));


		//Graph.ConnectionLogic(level.levelRoomList, level);

        for (int i = 0; i < pickedRooms.Count; i++)
        {
			//Debug.Log(pickedRooms[i].status);
		}

		pickedRooms.AddRange(RoomBranch(level, pickedRooms, center, 3, 3));
		//Forestation(level, pickedRooms);


		//Graph.ConnectionLogic(level.levelRoomList, level);


		return pickedRooms;
    }

	private List<Room> RoomBranch(Level level, List<Room> roomList, Vector2 center, int steps, int branchCount)
    {
		//Graph.ConnectionLogic(level.levelRoomList, level);

		List<Room> pickedRooms = new List<Room>();
		Room nextRoom; Room currentRoom;

		List<int> indexList = new List<int>();
		int poo = 0;
		// --------- PICKING SEED ROOMS FROM THE MAIN LOOP -----------------
        while (indexList.Count < branchCount && poo < 20) {
			poo++;
			int index = UnityEngine.Random.Range(0, roomList.Count);
			if (!indexList.Contains(index) && roomList[index].status == "LOOP") {
				indexList.Add(index);
			}
		}
		
        for (int i = 0; i < indexList.Count; i++) {
			indexList[i] = roomList[indexList[i]].indexRL;
		}
		// ------------------END-----------------

        foreach (var index in indexList)
        {
			//Debug.Log(level.levelRoomList[index].connectedRooms.Count() + " " + index + " " + level.levelRoomList[index].center());
			currentRoom = level.levelRoomList[index];

			for (int i = 0; i < steps; i++) {
				List<float> NeighA = new List<float>();


				string fff = "";
				for (int q = 0; q < currentRoom.connectedRooms.Count; q++) {
					
					Room checkRoom = level.levelRoomList[currentRoom.connectedRooms[q].indexRL];
					float rAngle = -600;
                    if (checkRoom.status != "LOOP" && checkRoom.status != "BRANCH" /*&& !pickedRooms.Contains( checkRoom)*/)
                    {
						rAngle = distanceBetweenPoints(checkRoom.center(), center);
					}
					fff = fff + " | " + rAngle;
					NeighA.Add(rAngle);
					

				}

				nextRoom = level.levelRoomList[ currentRoom.connectedRooms[NeighA.FindIndex(a => a == NeighA.AsQueryable().Max())].indexRL];

				if (nextRoom.status == "BRANCH" || nextRoom.status == "LOOP")
				{
					level.levelRoomList[currentRoom.indexRL] = currentRoom.ChangeType(new FarmFieldsRoom(level, 0, 0, 0, 0));
					level.levelRoomList[currentRoom.indexRL].status = "BRANCH";
					break;
				}

				if (i < steps -1)
				{
					level.levelRoomList[nextRoom.indexRL] = nextRoom.ChangeType(new MiscBranchRoom(level, 0, 0, 0, 0));
					level.levelRoomList[nextRoom.indexRL].status = "BRANCH";
				}
				else
				{
					level.levelRoomList[nextRoom.indexRL] = nextRoom.ChangeType(new FarmFieldsRoom(level, 0, 0, 0, 0));
					level.levelRoomList[nextRoom.indexRL].status = "BRANCH";
				}

				pickedRooms.Add(nextRoom);

				currentRoom = nextRoom;
			}
			/*
			for (int g = 0; g < pickedRooms.Count; g++)
			{
				level.levelRoomList[pickedRooms[g].indexRL] = pickedRooms[g].ChangeType(new MiscBranchRoom(level, 0, 0, 0, 0));
				level.levelRoomList[pickedRooms[g].indexRL].status = "BRANCH";
			}*/

		}
		return pickedRooms;
	}


	private List<Room> RoomGo(Level level, Room start, Room end)
    {
		Vector2 center = new Vector2((int)(level.sizeX / 2), (int)(level.sizeY / 2));
		List<Room> pickedRooms = new List<Room>();
		List<Vector2Int> points = CircleLevel(level, 2, 0.6f);

		Room currentRoom = start;
		int counter = 0;

		level.levelRoomList[start.indexRL] = start.ChangeType(new EntranceRoom(level, 0, 0, 0, 0));
		start = level.levelRoomList[start.indexRL];

		pickedRooms.Add(level.levelRoomList[start.indexRL]);
		//Debug.Log(level.levelRoomList[ level.levelRoomList.FindIndex(a => a.center() == start.center()) ]);

		while (currentRoom != end && counter < 25)
		{
			counter++;
			// Debug.Log(currentRoom.connectedRooms.Contains(end)); Debug.Log(currentRoom.center()); Debug.Log(currentRoom.connectedRooms.Count())
			if (currentRoom.connectedRooms.Contains(end)) {
                foreach (var item in pickedRooms) { item.status = "LOOP"; }
				Debug.Log("Finishg!!!!!!!!!!");
				break; }
			
			Room nextRoom = currentRoom;
			string angles = "";
			List<float> NeighA = new List<float>();

			if (distanceBetweenPoints(currentRoom.center(), end.center()) < 40) {
				for (int i = 0; i < currentRoom.connectedRooms.Count; i++)
                {
					NeighA.Add(angle3Points(currentRoom.connectedRooms[i].center(), end.center(), currentRoom.center() ));
					//Debug.Log( angle3Points(currentRoom.connectedRooms[i].center(), end.center(), currentRoom.center() ) + " " + currentRoom.connectedRooms[i].center()); ;
				}
				nextRoom = currentRoom.connectedRooms[NeighA.FindIndex(a => a == NeighA.AsQueryable().Min())];

				level.levelRoomList[nextRoom.indexRL] = nextRoom.ChangeType(new MiscRoom(level, 0, 0, 0, 0));
				level.levelRoomList[nextRoom.indexRL].status = "LOOP"; // Debug.Log(level.levelRoomList[i].status);
				pickedRooms.Add(level.levelRoomList[nextRoom.indexRL]);
				currentRoom = nextRoom;
			}


			else {
				for (int i = 0; i < currentRoom.connectedRooms.Count; i++) {
					float DistBetw = distanceBetweenPoints(currentRoom.connectedRooms[i].center(), center);

					if (DistBetw > distanceBetweenPoints(start.center(), center) * 0.3f && DistBetw < distanceBetweenPoints(start.center(), center) * 1.3f && !pickedRooms.Contains(currentRoom.connectedRooms[i]))
					{
						NeighA.Add(angle3Points(currentRoom.connectedRooms[i].center(), end.center(), center));
					}
					else {
						NeighA.Add(720);
					}
				}

				for (int i = 0; i < NeighA.Count; i++) { angles = angles + i + " : " + NeighA[i] + " ||| "; }
				nextRoom = currentRoom.connectedRooms[NeighA.FindIndex(a => a == NeighA.AsQueryable().Min())];

				level.levelRoomList[nextRoom.indexRL] = nextRoom.ChangeType(new MiscRoom(level, 0, 0, 0, 0));
				level.levelRoomList[nextRoom.indexRL].status = "LOOP"; // Debug.Log(level.levelRoomList[i].status);
				pickedRooms.Add(level.levelRoomList[nextRoom.indexRL]);
				currentRoom = nextRoom;
				//Debug.Log(currentRoom.connectedRooms.Count()); Debug.Log(currentRoom.center()); Debug.Log(currentRoom.connectedRooms.Contains(end));

			}
		}
		return pickedRooms;
	}

	public List<Room> Forestation(Level level, List<Room> loopR)
    {
		List<Room> forest = new List<Room>();

		List<int> loopRIndex = new List<int>();

		List<Room> openList = new List<Room>();
		List<Room> closedList = new List<Room>();
		//Debug.Log(level.levelRoomList.Count());

		for (int i = 0; i < loopR.Count; i++)
		{
			loopRIndex.Add( level.levelRoomList.FindIndex(a => a.center() == loopR[i].center())  );
		}


		foreach (var room in level.levelRoomList) {
            if (room.left == 0 || room.bottom == 0 || room.right == level.sizeX - 1 || room.top == level.sizeY - 1) {

				if (!loopR.Contains(room)) {
					openList.Add(room);
					//Debug.Log(room.Width() + " " + room.Height());
					
				}
			}
		}

		Room seed = openList[0];
		//closedList.Add(openList[0]);
		openList.Clear();
		openList.Add(seed);
		int counter = 0;
        while (openList.Count > 0 && counter < 15)
        {
			counter++;

            for (int i = 0; i < openList.Count; i++) {
				foreach (var neigh in openList[i].connectedRooms) {
					if (!openList.Contains(neigh) && !closedList.Contains(neigh)) {
						bool diff = true;
						foreach (var item in loopR) {
							if (SameRoom(item, neigh)) {
								diff = false;
							}
						}

						if (diff) {
							openList.Add(neigh);
						}
					}
				}
				closedList.Add(openList[i]);
				openList.Remove(openList[i]);
			}
        }

		for (int i = 0; i < closedList.Count; i++)
        {
			int index;
			index = level.levelRoomList.FindIndex(a => a.center() == closedList[i].center());
			if (loopR.Intersect(level.levelRoomList[index].connectedRooms).Count() > 0)
			{
				level.levelRoomList[index] = level.levelRoomList[index].ChangeType(new FarmFieldsRoom(level, 0, 0, 0, 0));
			}
			else
			{
				level.levelRoomList[index] = level.levelRoomList[index].ChangeType(new ForestRoom(level, 0, 0, 0, 0));
			}
		}

		return forest;
    }

	public bool SameRoom(Room roomA, Room roomB)
    {
		//Debug.Log( (roomA.bottom == roomB.bottom) + " " + (roomA.left == roomB.left) + " " + (roomA.right == roomB.right) + " " + (roomA.top == roomB.top) );
		return (roomA.bottom == roomB.bottom) && (roomA.left == roomB.left) && (roomA.right == roomB.right) && (roomA.top == roomB.top);

	}

	public Vector2Int RotateAround(Vector2 center, int angle, Vector2 p)
	{
		float s = (float)Math.Sin(PointAngle(angle));
		float c = (float)Math.Cos(PointAngle(angle));

		p.x = p.x - center.x;
		p.y = p.y - center.y;

		float xnew = p.x * c - p.y * s;
		float ynew = p.x * s + p.y * c;

		return new Vector2Int( (int)Math.Round(xnew + center.x), (int)Math.Round(ynew + center.y));
	}

	public Room PickRoom(Vector2Int point, Level level)
	{
		Room myRoom;

		for (int i = 0; i < level.levelRoomList.Count; i++)
		{

			if ( new Rect(level.levelRoomList[i].bottom, level.levelRoomList[i].left, level.levelRoomList[i].top -1, level.levelRoomList[i].right -1).inside(point.x, point.y) && !level.levelRoomList[i].hasChildren)
			{
				myRoom = level.levelRoomList[i];
				level.levelRoomList[i] = level.levelRoomList[i].ChangeType( new EntranceRoom(level, 0,0,0,0) );

				return myRoom;
			}
		}
		return null;
	}

	public List<Room> PickCircleRooms(List<Room> roomList, List<Vector2Int> pointsList, Level level)
    {
		List<Room> circleRoom = new List<Room>();

        for (int i = 0; i < roomList.Count; i++) {
            for (int j = 0; j < pointsList.Count; j++) {
                if (roomList[i].CheckIntersect( new Rect(pointsList[j].x - 3, pointsList[j].y - 3, pointsList[j].x + 3, pointsList[j].y + 3) ) && !roomList[i].hasChildren )
                {
					circleRoom.Add(roomList[i]);
					for (int q = 0; q < pointsList.Count; q++)
                    {
						if (roomList[i].shrink(2).inside(pointsList[q].x, pointsList[q].y) )
                        {
							pointsList.RemoveAt(q);
                        }

                    }

                }

            }
        }
		//Debug.Log(itCount);
		return circleRoom;
    }

	public Vector2 RoomCoordGen(Vector2 center, float coreAngle, int distance)
	{
		float value1 = center.x + (float)(distance * Math.Cos(PointAngle(coreAngle)));
		float value2 = center.y + (float)(distance * Math.Sin(PointAngle(coreAngle)));
		Vector2 roomCoords = new Vector2(value1, value2);
		return roomCoords;
	}

	public float PointAngle(float angle)
	{
		return (float)(Math.PI / 180 * (angle % 360));
	}

	public float angleBetweenPoints(Vector2 from, Vector2 to, bool allowNegatives = false)
	{


		float angle = (float)( 180 / Math.PI * (Math.Atan2(to.y - from.y, to.x - from.x) + Math.PI /2 ));

		if (angle < 0 && !allowNegatives) { angle = angle + 360;  }

		return angle;
	}
	public float angle3Points(Vector2 from, Vector2 to, Vector2 center)
	{
		float angle1 = angleBetweenPoints(from, center);
		float angle2 = angleBetweenPoints(to, center);
		if ((angle2 - angle1) < 0)
        {
			return angle2 - angle1 + 360;
		} else
        {
			return angle2 - angle1;
		}
		//return Math.Abs( Math.Min(  angle1 - angle2, angle2 + 360 - angle1) );
	}

	public float distanceBetweenPoints(Vector2 from, Vector2 to)
    {
		return (float)Math.Sqrt( (to.x - from.x)* (to.x - from.x) + (to.y - from.y ) * (to.y - from.y)) ;
    }

	public List<Vector2Int> CircleLevel(Level level, int PointCount, float distMod)
	{
		//float Height = UnityEngine.Random.Range(0.6f, 1f);
		float Height = 1f;
		float startAngle = UnityEngine.Random.Range(0, 360);
		List<Vector2Int> PathPoints = new List<Vector2Int>();

		Vector2 center = new Vector2((int)(level.sizeX / 2), (int)(level.sizeY / 2));
		float distance = center.x * distMod;
		Vector2Int pointOne = new Vector2Int((int)Math.Round(center.x - distance), (int)Math.Round(center.y));

		PathPoints.Add(pointOne);

		for (int i = 1; i < PointCount; i++)
		{
			Vector2 nextPoint = new Vector2(0f, 0f);
			nextPoint = RoomCoordGen(center, 180 + (i) * (360 / (PointCount)), (int)Math.Round(distance));

			PathPoints.Add(new Vector2Int((int)Math.Round(nextPoint.x), (int)Math.Round(center.y - (center.y - nextPoint.y) * Height)));
			//Debug.Log(nextPoint);
		}
		int rotateBy = UnityEngine.Random.Range(0, 360);


		for (int i = 0; i < PathPoints.Count; i++)
		{
			PathPoints[i] = RotateAround(center, rotateBy, PathPoints[i]);
		}

		return PathPoints;
	}
}
/*
for (int i = 0; i < 6; i++)
{
	for (int j = 0; j < 6; j++)
	{
		Vector2 coord1 = RoomCoordGen(center, i * 360 / 6, 15);
		Vector2 coord2 = RoomCoordGen(center, j * 360 / 6, 15);
		Debug.Log(coord1 + " " + coord2 + " " + i + " " + j);
		Debug.Log(angle3Points(coord1, coord2, center));
	}

}*/