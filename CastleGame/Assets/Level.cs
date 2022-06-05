using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class Level : AbstractDungeonGenerator
{
    [SerializeField]
    public GameObject ghost;
    public TileLib library;
    public GameObjectLib objectLib;
    /*
    [SerializeField]
    public Tilemap roomTileMapF, roomTileMapW;
    [SerializeField]
    public TileBase baseTile, wallTile;
    */
    [SerializeField]
    public GridNode[,] grid;
    public Vector2Int originPoint = new Vector2Int(0, 0);
    public int sizeX;
    public int sizeY;
    //public List<DungeonRoom> levelRooms = new List<DungeonRoom>();
    public List<Room> loopRooms = new List<Room>();
    public List<Room> levelRoomList = new List<Room>();
    public List<Room> levelRoomParents = new List<Room>();


    public List<GameObject> levelInteractibles = new List<GameObject>();

    public Vector2Int startNode, endNode;
    public List<GridNode> path;

    public Vector2Int CastleSize;

    public void CreateGrid()
    {
        interactiblesClear();
        loopRooms.Clear();
        levelRoomList.Clear();
        levelRoomParents.Clear();
        grid = new GridNode[sizeX, sizeY];
        //Debug.Log(sizeX + " " + sizeY);
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector2Int coords = new Vector2Int(originPoint.x + x, originPoint.y + y);
                bool walkable = true;
                grid[x, y] = new GridNode(walkable, coords, x, y, "BLACK");
                grid[x, y].gCost = 100;
            }
        }
        
        Vector2Int center = new Vector2Int( sizeX /2, sizeY / 2 );
        List<Room> a = new TetrisGenerator().GenerateRooms(this, center);
        levelRoomList.AddRange(a);

        //RoomPicker roomPick = new RoomPicker();

        HashSet<int> randRooms = new HashSet<int>();
        int emerCount = 0;
        /*
        for (int i = 0; i < levelRoomList.Count; i++) {
            if (levelRoomList[i].Width() > 8 && levelRoomList[i].Height() > 8) {
                Room tempRoom = levelRoomList[i];
                levelRoomList[i] = levelRoomList[i].ChangeType(new ExitRoom(this, tempRoom.left, tempRoom.bottom, tempRoom.right, tempRoom.top));
                return;
            }
        }*/

        while (emerCount < 160 && randRooms.Count < 8)
        {
            emerCount++;
            randRooms.Add(UnityEngine.Random.Range(0, this.levelRoomList.Count - 1));
        }
        List<int> cand = new List<int>();
        cand.AddRange(randRooms);

        Graph.ConnectionLogic(levelRoomList, this, true);

        for (int i = 0; i < levelRoomList.Count; i++)
        {
            if (levelRoomList[i].connectedRooms.Count == 0) { levelRoomList.RemoveAt(i); }
        }

        for (int i = 0; i < levelRoomList.Count; i++)
        {
            levelRoomList[i].indexRL = i;
        }

        

        Room aaa = levelRoomList[cand[0]];
        levelRoomList[cand[0]] = levelRoomList[cand[0]].ChangeType(new KeyStorageRoom(this, aaa.left, aaa.bottom, aaa.right, aaa.top));
        aaa = levelRoomList[cand[1]];
        levelRoomList[cand[1]] = levelRoomList[cand[1]].ChangeType(new KeyStorageRoom(this, aaa.left, aaa.bottom, aaa.right, aaa.top));
        aaa = levelRoomList[cand[2]];
        levelRoomList[cand[2]] = levelRoomList[cand[2]].ChangeType(new ExitRoom(this, aaa.left, aaa.bottom, aaa.right, aaa.top));
        aaa = levelRoomList[cand[3]];
        levelRoomList[cand[3]] = levelRoomList[cand[3]].ChangeType(new KeyStorageRoom(this, aaa.left, aaa.bottom, aaa.right, aaa.top));

        for (int i = 4; i < cand.Count; i++)
        {
            aaa = levelRoomList[cand[i]];
            levelRoomList[cand[i]] = levelRoomList[cand[i]].ChangeType(new ResourceRoom(this, aaa.left, aaa.bottom, aaa.right, aaa.top));
        }



        Graph.ConnectionLogic(levelRoomList, this, true);

        //loopRooms = roomPick.LoopLevel(this);



        




        foreach (var item in levelRoomList) {
            item.GetTiles();
            //Debug.Log(item.indexRL);
        }

        levelRoomList[cand[0]].freeSpot();

        DoorPropagation.PropagateDoors(levelRoomList, this);
        List<Room> usedRoom = new List<Room>();
        /*
        for (int i = 0; i < levelRoomList.Count; i++) {
            
            foreach (var neighbour in levelRoomList[i].connectedRooms) {
                if (!usedRoom.Contains(neighbour) && levelRoomList.Contains(neighbour) ) {
                    RoomPainterBrushes.Line(this, neighbour.center().x, neighbour.center().y, levelRoomList[i].center().x, levelRoomList[i].center().y, "DECO");
                }
            }
            usedRoom.Add(levelRoomList[i]);
        }*/
        library.PaintLevel(this);
        /*
        int rand = UnityEngine.Random.Range(0, levelRoomList.Count - 1);
        GameObject e = Instantiate(square);
        e.transform.position = new Vector3(levelRoomList[rand].center().x, levelRoomList[rand].center().y, 1);*/



        int randRoom = UnityEngine.Random.Range(1, this.levelRoomList.Count - 1);
        GridNode tempNode = this.levelRoomList[randRoom].QuickFreeSpot();
        Vector3 b = GridToWorld( new Vector2Int( tempNode.gridX, tempNode.gridY));

        ghost.transform.position = new Vector3(b.x,b.y,0);
    }

    private List<Vector2Int> directionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1),    //up
        new Vector2Int(1,0),    //right
        new Vector2Int(0,-1),   //down
        new Vector2Int(-1,0)    //left
    };

    public List<GridNode> GetNeighbourds(GridNode node)
    {
        List<GridNode> neighbourds = new List<GridNode>();

        foreach (var direction in directionsList)
        {
            int checkX = node.gridX + direction.x;
            int checkY = node.gridY + direction.y;
            if (checkX >= 0 && checkX < sizeX && checkY >= 0 && checkY < sizeY)
            {
                neighbourds.Add(grid[checkX, checkY]);
            }
        }
        //foreach (var item in neighbourds) { Debug.Log("Neighbours: " + item.gridX + " " + item.gridY); }
        return neighbourds;
    }

    public List<GridNode> GetNeighbourds8(GridNode node)
    {
        List<GridNode> neighbourds = new List<GridNode>();
        int nX = node.gridX;
        int nY = node.gridY;
        int snX = nX;
        int enX = nX;
        int snY = nY;
        int enY = nY;

        if (nX + 1 <= sizeX - 1) {
            if (grid[nX + 1, nY].walkable) {
                enX = nX + 1;
            } // X up
        }
        if (nX - 1 >= 0) {
            if (grid[nX - 1, nY].walkable) {
                snX = nX - 1;
            } // X down
        }
        if (nY + 1 <= sizeY - 1) {
            if (grid[nX, nY + 1].walkable) {
                enY = nY + 1;
            } // Y up
        }
        if (nY - 1 >= 0) {
            if (grid[nX, nY - 1].walkable) {
                snY = nY - 1;
            } // Y down
        }



        /*
        snX = Mathf.Clamp(nX - 1, 0, sizeX - 1);
        enX = Mathf.Clamp(nX + 1, 0, sizeX - 1);
        snY = Mathf.Clamp(nY - 1, 0, sizeX - 1);
        enY = Mathf.Clamp(nY + 1, 0, sizeX - 1);
        */
        for (int i = snX; i < enX + 1; i++) {
            for (int j = snY; j < enY + 1; j++) {
                if (!(i == nX && j == nY) ) {
                    neighbourds.Add(this.grid[i, j]);
                    //Debug.Log(i + " " + j);
                }
            }
        }

        
        return neighbourds;
    }


    public GridNode WorldToGrid(Vector3 point) {
        Vector3 scale = library.globalGrid.transform.localScale;
        float percentX = (point.x) / sizeX / scale.x;
        float percentY = (point.y) / scale.y / sizeY;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = (int)Math.Floor((sizeX) * percentX);
        int y = (int)Math.Floor((sizeY) * percentY);
        if (x == sizeX) { x = x - 1; }
        if (y == sizeY) { y = y - 1; }
        return grid[x, y];
    }

    public Vector2 GridToWorld(Vector2Int point) {
        Vector3 scale = library.globalGrid.transform.localScale;
        float X = (point.x + 0.5f) * scale.x;
        float Y = (point.y + 0.5f) * scale.y;
        return new Vector2(X, Y);
    }

    public void interactiblesClear() {
        foreach (var item in levelInteractibles) {
            DestroyImmediate(item);
        }
        levelInteractibles.Clear();
    }


    protected override void RunLevelGeneration() {
        library.ClearAll();
        CreateGrid();
    }
    protected override void LevelClear() {
        library.ClearAll();
        loopRooms.Clear();
        levelRoomList.Clear();
        interactiblesClear();
    }
    protected override void ObjectsClear() {
        interactiblesClear();
    }

    public float distanceBetweenPoints(Vector2 from, Vector2 to) {
        return (float)Math.Sqrt((to.x - from.x) * (to.x - from.x) + (to.y - from.y) * (to.y - from.y));
    }
}

/*
HallGenerator hallway = new HallGenerator();
hallway.grid = this;

BasicDungeon dungeon = new BasicDungeon(this);
dungeon.GenerateLoopLevel();

List<GridNode> List1 = new List<GridNode>(); List<GridNode> List2 = new List<GridNode>();
List1.Add(grid[0,0]); List2.Add(grid[9, 3]);


foreach (DungeonRoom room in levelRooms)
{
    room.GetTiles();
    Debug.Log("Coords: "+ room.coords + " Width: " + room.roomWidth + " Height: " + room.roomHeight);
}

for (int j = 0; j < 5; j++)
{
    grid[4, 0 + j].walkable = false;
    grid[5, 0 + j].walkable = false;
}
grid[4, 0].walkable = true;
grid[4, 0].hCost = 5;
grid[5, 0].walkable = true;*/

/*hallway.Pathfind(List1, List2);

HashSet<Vector2Int> passageWay = new HashSet<Vector2Int>();

foreach (var item in path)
{
    passageWay.Add(item.worldPos);
}*/

//OpenTilePainter.PaintTiles(roomTileMapW, wallTile, passageWay);

/*
 for (int i = 0; i < levelRoomList.Count; i++)
        {
            if (levelRoomList[i].inside(dotty.x, dotty.y) && !levelRoomList[i].hasChildren)
            {
                levelRoomList[i] = levelRoomList[i].ChangeType(new EntranceRoom(this, 0, 0, 0, 0));
                Debug.Log(levelRoomList[i] + "  " + levelRoomList[i].left + " " + levelRoomList[i].bottom + " " + levelRoomList[i].right + " " + levelRoomList[i].top);
                levelRoomList[i].GetTiles();
                return;
            }  
        }
 */

/*
        Vector2Int center = new Vector2Int(0 + sizeX /2, 0 + sizeY/2);
        Vector2Int bottomCastle = new Vector2Int(center.x - CastleSize.x / 2, center.y - CastleSize.y /2   );
        Vector2Int topCastle = new Vector2Int(center.x + CastleSize.x / 2, center.y + CastleSize.y / 2);

        BasicRoom seedRoom = new BasicRoom(this, bottomCastle.x, bottomCastle.y, topCastle.x - 1, topCastle.y - 1);
        List<Room> temp = new BinarySplit(this).BinarySpacePartitioning(seedRoom, needParents: false);  

        foreach (var room in temp) {
            if (!room.hasChildren) {
                levelRoomList.Add(room);
            } else {
                levelRoomParents.Add(room);
            }
        }
 */