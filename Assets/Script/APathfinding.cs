using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APathfinding : MonoBehaviour {

    public Transform seeker;
    public Transform target;
    public List<WorldTile> path;


    void Awake()
    {
    }
    void Start()
    {

    }
    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        WorldTile startNode = CreateNodesFromTilemaps.instance.NodeFromPosition(startPos);
        WorldTile targetNode = CreateNodesFromTilemaps.instance.NodeFromPosition(targetPos);
        Heap<WorldTile> openSet = new Heap<WorldTile>(CreateNodesFromTilemaps.instance.MaxSize);
        HashSet<WorldTile> closedSet = new HashSet<WorldTile>();
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            WorldTile currentWorldTile = openSet.RemoveFirst();
            closedSet.Add(currentWorldTile);
            if (currentWorldTile == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
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



    public void RetracePath(WorldTile startNode, WorldTile endNode)
    {
        path = new List<WorldTile>();
        WorldTile currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        CreateNodesFromTilemaps.instance.path = path;

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
