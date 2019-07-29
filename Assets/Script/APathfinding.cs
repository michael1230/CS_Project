using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class APathfinding : MonoBehaviour
{


    CreateNodesFromTilemaps grid;
    void Awake()
    {
        grid = GetComponent<CreateNodesFromTilemaps>();
    }


    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        WorldTile startNode = grid.NodeFromPosition(request.pathStart);
        WorldTile targetNode = grid.NodeFromPosition(request.pathEnd);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<WorldTile> openSet = new Heap<WorldTile>(grid.MaxSize);
            HashSet<WorldTile> closedSet = new HashSet<WorldTile>();
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                WorldTile currentWorldTile = openSet.RemoveFirst();
                closedSet.Add(currentWorldTile);
                if (currentWorldTile == targetNode)
                {
                    pathSuccess = true;
                    break;
                }
                foreach (WorldTile neighbour in currentWorldTile.getMyNeighbours())
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                    int newCostToNeighbour = currentWorldTile.gCost + GetDistance(currentWorldTile, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentWorldTile;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }



    Vector3[] RetracePath(WorldTile startNode, WorldTile endNode)
    {
        List<WorldTile> path = new List<WorldTile>();
        WorldTile currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        if (currentNode == startNode)
        {
            path.Add(currentNode);
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<WorldTile> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        if (path.Count == 1)
        {
            waypoints.Add(path[0].gridPosition);
            return waypoints.ToArray();
        }

        for (int i = 1; i < path.Count; i++)
        {
            waypoints.Add(path[i - 1].gridPosition);
        }
        return waypoints.ToArray();
    }

    public int GetDistance(WorldTile nodeA, WorldTile nodeB)
    {
        
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        else
        {
            return 14 * dstX + 10 * (dstY - dstX);
        }      
    }
}
