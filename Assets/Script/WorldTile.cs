using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile :IHeapItem<WorldTile>
{
    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;

    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    public int heapIndex;
    public Vector3 gridPosition;
    public bool walkable;
    public List<WorldTile> myNeighbours;
    public WorldTile parent;





    public WorldTile(bool walkable, Vector3 worldPos, int gridX, int gridY)
    {
        this.gridX = gridX;
        this.gridY = gridY;
        this.walkable = walkable;
        this.gridPosition = worldPos;
        this.xMin = gridPosition.x - 0.5f;
        this.xMax = gridPosition.x + 0.5f;
        this.yMin = gridPosition.y - 0.5f;
        this.yMax = gridPosition.y + 0.5f;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public List<WorldTile> getMyNeighbours()
    {
        return myNeighbours;
    }

    public int HeapIndex
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

    public int CompareTo(WorldTile nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }


}
