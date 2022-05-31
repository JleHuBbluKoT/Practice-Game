using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHall
{

    public Level grid;

    public void PooPoo()
    {

        foreach (var item in grid.grid)
        {
            Debug.Log(item + " " + item.gridX + " " + item.gridY);
        }
    }

    public void Pathfind(GridNode startPos, GridNode targetPos)
    {
        GridNode startNode = startPos;
        GridNode targetNode = targetPos;

        List<GridNode> openSet = new List<GridNode>();
        HashSet<GridNode> closedSet = new HashSet<GridNode>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GridNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost)
                { 
                    if (openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                    
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                Debug.Log("Path found!");
                RetracePath(startNode, targetNode);
                return;
            }
            
            foreach (GridNode neighbour in grid.GetNeighbourds(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newMovemtntCostToNeighbour = currentNode.gCost + GetDist(currentNode, neighbour);

                if (newMovemtntCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovemtntCostToNeighbour;
                    neighbour.hCost = GetDist(neighbour, targetNode);
                    neighbour.parent = currentNode;
                    openSet.Add(neighbour);
                }
            }
        }


    }
    
    public void RetracePath(GridNode startNode, GridNode targetNode)
    {
        List<GridNode> path = new List<GridNode>();
        GridNode currentNode = targetNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
    }

    public int GetDist(GridNode nodeA, GridNode nodeB)
    {
        return Mathf.Abs(nodeA.gridX - nodeB.gridX) + Mathf.Abs(nodeA.gridY - nodeB.gridY);
    }
}
