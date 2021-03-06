using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Creature : MonoBehaviour
{
    public Level level;
    public GameObject playerR;
    public bool Invincible;
    public float health = 100;
    public int MaxHealth = 100;



    public List<GridNode> pathNodes;
    //protected string state;
    //protected float speed;
    //Rigidbody2D rb;
    void Start() {
        //rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player    

        //pathNodes = new List<GridNode>();
    }

    // Update is called once per frame
    void Update()
    {
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


    public List<GridNode> AStar(Vector2Int start, Vector2Int end, bool deleteUnnecesary = true)
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

            currentNode = openList.Aggregate((min, x) => x.fCost < min.fCost ? x : min); //???? ?????????? fCost 
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode) {
                //Debug.Log("Path found!");
                path = RetracePath(level.grid[start.x, start.y], endNode);
                if (deleteUnnecesary)
                {
                    path = PathSimplification(path);
                }
                
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
        //Debug.Log("?? ??????????");
        path = new List<GridNode>();
        path.Clear();
        path.Add(level.grid[start.x, start.y]);
        return path;
    }

    public List<GridNode> PathSimplification(List<GridNode> path ) {
        List<GridNode> SimplePath = new List<GridNode>();

        List<int> killList = new List<int>();
        // ?????? ???????????????? ???? ? ?????????? ????????????
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

    public float distance(GameObject other)
    {
        return Mathf.Sqrt((float)Math.Pow(transform.position.x - other.transform.position.x, 2) + (float)Math.Pow(transform.position.y - other.transform.position.y, 2));
    }

    public virtual void ApplyDamage(float Damage)
    {
        if (!Invincible)
        {
            health = Mathf.Clamp(health - Damage, 0, MaxHealth);
        }

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
