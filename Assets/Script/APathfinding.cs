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
        FindPath(seeker, target);


        foreach (GameObject n in CreateNodesFromTilemaps.instance.nodes)
        {           
            if(n!=null)
            {
                WorldTile wt = n.GetComponent<WorldTile>();
                if (path.Contains(wt))
                    wt.GetComponent<SpriteRenderer>().color = Color.black;
                else if (wt.walkable == false)
                    wt.GetComponent<SpriteRenderer>().color = Color.red;
                else
                    wt.GetComponent<SpriteRenderer>().color = Color.white;
            }

        }



    }

    public void FindPath(Transform startPos, Transform targetPos)
    {
        WorldTile startNode = CreateNodesFromTilemaps.instance.NodeFromPosition(startPos.position);
        WorldTile targetNode = CreateNodesFromTilemaps.instance.NodeFromPosition(targetPos.position);

        //List <WorldTile> openSet = new List<WorldTile>();
        Heap<WorldTile> openSet = new Heap<WorldTile>(CreateNodesFromTilemaps.instance.MaxSize);

        HashSet<WorldTile> closedSet = new HashSet<WorldTile>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            /*WorldTile currentWorldTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentWorldTile.fCost || openSet[i].fCost == currentWorldTile.fCost)
                {
                    if (openSet[i].hCost < currentWorldTile.hCost)
                        currentWorldTile = openSet[i];
                }
            }

            openSet.Remove(currentWorldTile);
            closedSet.Add(currentWorldTile);

            if (currentWorldTile == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }
            */
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

    // Use this for initialization


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

        foreach (WorldTile neighbour in path)
        {
            neighbour.GetComponent<SpriteRenderer>().color = Color.black;
        }


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
