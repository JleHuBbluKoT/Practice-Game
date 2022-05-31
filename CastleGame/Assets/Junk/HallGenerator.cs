using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HallGenerator
{
    public Level grid;

    public void Pathfind(List<GridNode> RoomA, List<GridNode> RoomB)
    {
        List<GridNode> openSet = new List<GridNode>();
        List<GridNode> closedSet = new List<GridNode>();
        List<GridNode> lowerSet = new List<GridNode>();

        openSet.AddRange(RoomA);
        foreach (var item in openSet) { item.gCost = 0;}


        int failCheck = 0;
        while (openSet.Count > 0 && failCheck < 300)
        {
            failCheck++;
            int minCost = openSet[0].gCost;
            for (int i = 0; i < openSet.Count(); i++)
            {
                if (openSet[i].gCost < minCost)
                {
                    minCost = openSet[i].gCost;
                }
            }
            //Debug.Log("__________Potential_List___________");
            foreach (var node in openSet) {
                if (node.gCost == minCost) {
                    lowerSet.Add(node);
                    //Debug.Log(node.gridX + " " + node.gridY);
                }
            }
            //Debug.Log("__________________________________");


            foreach (var node in lowerSet) {
                openSet.Remove(node);
                closedSet.Add(node);
                if (RoomB.Contains(node)) {
                    Debug.Log(node.gridX + " " + node.gridY);
                    Debug.Log("Path found!");
                    RetracePath(node);
                    return;
                }

                foreach (var neighbour in grid.GetNeighbourds(node)) {

                    if (!neighbour.walkable || closedSet.Contains(neighbour)) { continue; }

                    int newMovemtntCostToNeighbour = node.gCost + 1 + neighbour.hCost;

                    if (!openSet.Contains(neighbour)) {
                        neighbour.gCost = newMovemtntCostToNeighbour;
                        openSet.Add(neighbour);
                    }
                }  
            }
            lowerSet.Clear();
        }
        Debug.Log("fail!");
    }

    public void RetracePath(GridNode startNode)
    {
        List<GridNode> path = new List<GridNode>();
        GridNode currentNode = startNode;
        Vector2Int favDirect = new Vector2Int(0,0);
        int failCheck = 0;
        while (currentNode.gCost != 0 && failCheck < 200 )
        {
            failCheck++;
            path.Add(currentNode);

            if (currentNode.gridX + favDirect.x >= 0 && currentNode.gridX + favDirect.x < grid.sizeX && currentNode.gridY + favDirect.y >= 0 && currentNode.gridY + favDirect.y < grid.sizeY && (grid.grid[currentNode.gridX + favDirect.x, currentNode.gridY + favDirect.y].gCost < currentNode.gCost))
            {
                currentNode = grid.grid[currentNode.gridX + favDirect.x, currentNode.gridY + favDirect.y];
            }

            else
            {
                foreach (var item in grid.GetNeighbourds(currentNode))
                {
                    if (item.gCost < currentNode.gCost)
                    {
                        Vector2Int JoeBiden = new Vector2Int(item.gridX - currentNode.gridX, item.gridY - currentNode.gridY);
                        favDirect = JoeBiden;
                        currentNode = item;
                    }
                }
            }
        }
        path.Add(currentNode);
        grid.path = path;
    }

}
