﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class APathfinding : MonoBehaviour {

    //public Transform seeker;
    //public Transform target;

    PathRequestManager requestManager;
    CreateNodesFromTilemaps grid;
    public List<WorldTile> path;


    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<CreateNodesFromTilemaps>();
    }
    void Start()
    {

    }
    void Update()
    {
        //FindPath(seeker.position, target.position);
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    //public void FindPath(Vector3 startPos, Vector3 targetPos)
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        /*
        WorldTile startNode = CreateNodesFromTilemaps.instance.NodeFromPosition(startPos);
        WorldTile targetNode = CreateNodesFromTilemaps.instance.NodeFromPosition(targetPos);
        */

        WorldTile startNode = grid.NodeFromPosition(startPos);
        WorldTile targetNode = grid.NodeFromPosition(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            //Heap<WorldTile> openSet = new Heap<WorldTile>(CreateNodesFromTilemaps.instance.MaxSize);
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
                    //RetracePath(startNode, targetNode);
                    //return;
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
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }



    //public void RetracePath(WorldTile startNode, WorldTile endNode)
    Vector3[] RetracePath(WorldTile startNode, WorldTile endNode)
    {
        path = new List<WorldTile>();
        WorldTile currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        //path.Reverse();
        //CreateNodesFromTilemaps.instance.path = path;
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<WorldTile> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        if(path.Count==1)
        {
            waypoints.Add(path[0].gridPosition);
            return waypoints.ToArray();
        }

        for (int i = 1; i < path.Count; i++)
        {
            //Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            //if (directionNew != directionOld)
            //{
                waypoints.Add(path[i-1].gridPosition);
            //}
            //directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    public int GetDistance(WorldTile nodeA, WorldTile nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
