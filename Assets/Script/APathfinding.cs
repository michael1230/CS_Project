using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class APathfinding : MonoBehaviour
{
    CreateNodesFromTilemaps grid;//the CreateNodesFromTilemaps object itself
    void Awake()
    {
        grid = GetComponent<CreateNodesFromTilemaps>();
    }
    public void FindPath(PathRequest request, Action<PathResult> callback)//a method to find the A* path
    {
        Vector3[] waypoints = new Vector3[0];//the Vector3 array which will be the path
        bool pathSuccess = false;//a bool which tell us if we have successfully found a path
        WorldTile startNode = grid.NodeFromPosition(request.pathStart);//the start node
        WorldTile targetNode = grid.NodeFromPosition(request.pathEnd);//the target node
        if (startNode.walkable && targetNode.walkable)//if the enemy is able to walk on the target and start
        {
            Heap<WorldTile> openSet = new Heap<WorldTile>(grid.MaxSize);//the open set of WorldTile
            HashSet<WorldTile> closedSet = new HashSet<WorldTile>();//the closed set of WorldTile
            openSet.Add(startNode);//add the start node
            while (openSet.Count > 0)//while the open set is not empty
            {
                WorldTile currentWorldTile = openSet.RemoveFirst();//get the WorldTile
                closedSet.Add(currentWorldTile);//add it to the close set
                if (currentWorldTile == targetNode)//if the currentWorldTile is the targetNode then we have found the A* path
                {
                    pathSuccess = true;
                    break;
                }
                foreach (WorldTile neighbour in currentWorldTile.getMyNeighbours())//for each neighbor of the currentWorldTile
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))//if the neighbor is not walkable or it is already in the close set
                    {
                        continue;//skip the current iteration
                    }
                    int newCostToNeighbour = currentWorldTile.gCost + GetDistance(currentWorldTile, neighbour);//the g cost of this neighbour 
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))//if we have found better path to this neighbour or the neighbour is not in the open set
                    {
                        neighbour.gCost = newCostToNeighbour;//update the g cost
                        neighbour.hCost = GetDistance(neighbour, targetNode);//calculate the h cost
                        neighbour.parent = currentWorldTile;//set the parent of the neighbor to currentWorldTile
                        if (!openSet.Contains(neighbour))//if the neighbor is not in the open set
                            openSet.Add(neighbour);//add him
                        else//if he is then we need to update him
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }
        if (pathSuccess)//if we found a path
        {
            waypoints = RetracePath(startNode, targetNode);//get the Vector3 path
            pathSuccess = waypoints.Length > 0;//if we have a path then true
        }
        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }
    Vector3[] RetracePath(WorldTile startNode, WorldTile endNode)//a method to get the A* complete path
    {
        List<WorldTile> path = new List<WorldTile>();//a list of WorldTile which will be the path
        WorldTile currentNode = endNode;//save the target node
        while (currentNode != startNode)//while we have not reached the startNode
        {
            path.Add(currentNode);//add the currentNode
            currentNode = currentNode.parent;//the currentNode now is the parent of the previous currentNode
        }
        if (currentNode == startNode)//if we have reached the startNode
        {
            path.Add(currentNode);//add him
        }
        Vector3[] waypoints = SimplifyPath(path);//make a Vector3 path from the WorldTile
        Array.Reverse(waypoints);//revers it because the path is from the target to the start
        return waypoints;//return the path
    }
    Vector3[] SimplifyPath(List<WorldTile> path)//a method to Vector3 path from the WorldTile
    {
        List<Vector3> waypoints = new List<Vector3>();// a list of Vector3 which will be the path
        if (path.Count == 1)//if we only have one more node to move
        {
            waypoints.Add(path[0].gridPosition);//add the last node
            return waypoints.ToArray();//return array
        }
        for (int i = 1; i < path.Count; i++)//add the nodes
        {
            waypoints.Add(path[i - 1].gridPosition);
        }
        return waypoints.ToArray();      
    }
    public int GetDistance(WorldTile nodeA, WorldTile nodeB)//a method to calculate the distance between two nodes(g and h cost)
    {    
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (dstX > dstY)//the lowest is the number of diagonal moves 
        {
            return 14 * dstY + 10 * (dstX - dstY);//14 is th cost for diagonal and 10 is the cost for straight
        }
        else
        {
            return 14 * dstX + 10 * (dstY - dstX);//14 is th cost for diagonal and 10 is the cost for straight
        }      
    }
}
