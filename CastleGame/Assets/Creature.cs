using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Creature : MonoBehaviour
{
    public Level level;
    public GameObject playerR;
    public string state;
    public List<GridNode> pathNodes;
    

    public float speed = 2f;
    public Rigidbody2D rb;
    void Start() {
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player    

        pathNodes = new List<GridNode>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown( "r" )) {
            Vector2Int a =  new Vector2Int(level.WorldToGrid(this.transform.position).gridX, level.WorldToGrid(this.transform.position).gridY);
            Vector2Int b =  new Vector2Int(level.WorldToGrid(playerR.transform.position).gridX, level.WorldToGrid(playerR.transform.position).gridY);
            pathNodes = AStar(a, b);
        }

        if (pathNodes.Count > 0) {
            Vector2 nextNodePost = level.GridToWorld(new Vector2Int(pathNodes[0].gridX, pathNodes[0].gridY));
            rb.velocity = angleBetweenPoints(this.transform.position, nextNodePost) * speed;

            Vector2 targetNode = level.GridToWorld(new Vector2Int(pathNodes[0].gridX, pathNodes[0].gridY));


            if ( (Math.Abs(targetNode.x - transform.position.x) < 0.10f ) && (Math.Abs(targetNode.y - transform.position.y) < 0.10f))
            {
                pathNodes.RemoveAt(0);
            }
        } else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public Vector2 angleBetweenPoints(Vector2 from, Vector2 to)
    {
        Vector2 dirVector = new Vector2();
        float angle = (float)Math.Atan2(to.y - from.y, to.x - from.x);
        float x = Mathf.Clamp((float)Math.Cos(angle), -1, 1);
        float y = Mathf.Clamp((float)Math.Sin(angle), -1, 1);
        dirVector = new Vector2(x, y);
        return dirVector;
    }


    public List<GridNode> AStar(Vector2Int start, Vector2Int end)
    {
        List<GridNode> path;
        List<GridNode> openList = new List<GridNode>();
        HashSet<GridNode> closedList = new HashSet<GridNode>();
        GridNode currentNode = level.grid[start.x, start.y];
        GridNode endNode = level.grid[end.x, end.y];
        currentNode.gCost = 0;
        currentNode.setHueristics(end);
        openList.Add(currentNode);

        int counter = 0;
        while (openList.Count > 0 && counter < 1000) {
            counter++;

            currentNode = openList.Aggregate((min, x) => x.fCost < min.fCost ? x : min); //Ищет наименьший fCost 
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode) {
                Debug.Log("Path found!");
                path = RetracePath(level.grid[start.x, start.y], endNode);
                path = PathSimplification(path);
                Debug.Log(path.Count);
                return path;
            }

            foreach (var neigh in level.GetNeighbourds8(currentNode)) {
                if (!neigh.walkable || closedList.Contains(neigh)) { continue; }

                int newMovemtntCostToNeighbour = currentNode.gCost + simpleDist(currentNode.gridX, currentNode.gridY, new Vector2Int(neigh.gridX, neigh.gridY));
                if (newMovemtntCostToNeighbour < neigh.gCost || !openList.Contains(neigh)) {
                    neigh.gCost = newMovemtntCostToNeighbour;
                    neigh.hCost = simpleDist(neigh.gridX, neigh.gridY, new Vector2Int(endNode.gridX, endNode.gridY));
                    neigh.parent = currentNode;
                    //Debug.Log(neigh.parent.gridX + " " + neigh.parent.gridY);
                    openList.Add(neigh);
                }
            }
        }
        Debug.Log("Не получилось");
        return null;
    }

    public List<GridNode> PathSimplification(List<GridNode> path ) {
        List<GridNode> SimplePath = new List<GridNode>();

        List<int> killList = new List<int>();
        // Убираю последовательные ноды с одинаковым направлением
        Vector2Int curDir = Vector2Int.zero;
        Vector2Int prevDir = Vector2Int.zero;
        for (int i = 0; i < path.Count - 1; i++) {
            curDir = new Vector2Int(path[i].gridX - path[i + 1].gridX, path[i].gridY - path[i + 1].gridY);
            if (curDir == prevDir) {
                killList.Add(i);
            }
            prevDir = curDir;
        }
        killList.Reverse();
        foreach (var item in killList ) {
            path.RemoveAt(item);
        }
        SimplePath = path;
        /*
        Debug.Log("LIPOMA");
        foreach (var item in path) {
            Debug.Log(item.gridX + " " + item.gridY);
            item.type = "DECO";
        }
        level.library.PaintLevel(level);*/
        return SimplePath;
    }

    public List<GridNode> PathSimplificationRayCast(List<GridNode> path) {
        List<GridNode> SimplePath = new List<GridNode>();
        for (int i = 0; i < path.Count - 1; i++) { 
        }
        return SimplePath;
    }

    public bool CanGetTo(GridNode p1, GridNode p2)
    {
        Vector2 startP = level.GridToWorld(new Vector2Int(p1.gridX, p1.gridY));
        Vector2 startEP = level.GridToWorld(new Vector2Int(p2.gridX, p2.gridY));
        float dist = Mathf.Sqrt(startEP.x * startEP.x + startEP.y * startEP.y);


        Vector3 diff = new Vector3(startEP.x - startP.x, startEP.y - startP.y, 0);
        float distance = (float)(Mathf.Abs((float)(Mathf.Min(diff.x, diff.y) * 1.5)) + Mathf.Abs(diff.x - diff.y));

        RaycastHit2D hit = Physics2D.Raycast(startP, diff, distance);
        Debug.Log(diff);
        Debug.Log(distance);
        if (hit.collider.tag != "Wall")
        {
            Debug.Log("access");
            return true;
        }
        else { Debug.Log("WALL HERE!"); }
        return false;
    }

    public List<GridNode> RetracePath(GridNode startNode, GridNode targetNode)
    {
        List<GridNode> path = new List<GridNode>();
        GridNode currentNode = targetNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();
        return path;
    }

    public int simpleDist(int x, int y, Vector2Int target)
    {
        int dX = Mathf.Abs(x - target.x);
        int dY = Mathf.Abs(y - target.y);
        return Mathf.Min(dX, dY) * 4 + Mathf.Max(dX, dY) * 10;
    }
}
/*
                foreach (var item in closedList) {
                    item.type = "GRASS";
                }

                foreach (var item in path) {
                    Debug.Log(item.gridX + " " + item.gridY);
                    item.type = "GRASSSPECIAL";
                }
                level.library.PaintLevel(level); */
