using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile :IHeapItem<WorldTile>
{
    public int gCost;//the g cost of the node
    public int hCost;//the h cost of the node
    public int gridX;//the grid X position of the node(in the array of all nodes)
    public int gridY;//the grid Y position of the node(in the array of all nodes)
    public float xMin;//the min X position for the nod in its tile 
    public float xMax;//the max X position for the nod in its tile 
    public float yMin;//the min Y position for the nod in its tile 
    public float yMax;//the max Y position for the nod in its tile 
    public int heapIndex;////////////////////////////////////////////////////////////////////////////////////////////
    public Vector3 gridPosition;//the vector 3 grid position
    public bool walkable;//if this is a walkable node
    public List<WorldTile> myNeighbours;//a list of this node neighbors
    public WorldTile parent;//the parent of this node..when we to a path
    public WorldTile(bool walkable, Vector3 worldPos, int gridX, int gridY)//a parameter constructor
    {
        this.myNeighbours = new List<WorldTile>();
        this.gridX = gridX;
        this.gridY = gridY;
        this.walkable = walkable;
        this.gridPosition = worldPos;
        this.xMin = gridPosition.x - 0.5f;
        this.xMax = gridPosition.x + 0.5f;
        this.yMin = gridPosition.y - 0.5f;
        this.yMax = gridPosition.y + 0.5f;
    }
    public int fCost//the f cost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public List<WorldTile> getMyNeighbours()//getter
    {
        return myNeighbours;
    }
    public int HeapIndex//a getter or setter
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
    public int CompareTo(WorldTile nodeToCompare)//a comparator for the priority queue
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }


}
