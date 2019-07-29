using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class CreateNodesFromTilemaps : MonoBehaviour
{
    public bool displayGridGizmos;//a bool to know if to show gizmo
    //public Grid gridBase;//
    public Tilemap floor;//the walkable layer 
    private int maxXFloor;//the max X coordinates 
    private int maxYFloor;//the max Y coordinates 
    private int minXFloor;//the min X coordinates 
    private int minYFloor;//the min X coordinates 
    public List<Tilemap> obstacleLayers;//all layers that contain obstacles
    public int unwalkableNodeBorder = 1;//for test and not use in the final game
    //these are the bounds of where we are searching in the world for tiles, have to use world coords to check for tiles in the tile map..some maps have other numbers
    public int scanStartX = -250;
    public int scanStartY = -250;
    public int scanFinishX = -250;
    public int scanFinishY = -250;
    public List<WorldTile> unSortedNodes;//unSorted nodes
    public WorldTile[,] sortedNodes;//the sorted nodes
    public int gridBoundX = 0;//initialize  with 0
    public int gridBoundY = 0;//initialize  with 0
    // Use this for initialization
    void Awake()//do this first
    {
        unSortedNodes = new List<WorldTile>();
        maxXFloor = floor.size.x;
        maxYFloor = floor.size.y;
        minXFloor = floor.cellBounds.xMin;
        minYFloor = floor.cellBounds.yMin;
        createNodes();//call the createNodes method
    }
    // Update is called once per frame
    void Update()
    {

    }
    void Start()
    {

    }
    public WorldTile NodeFromPosition(Vector3 pos)//a method get the node from the position
    {
        int wxp = Mathf.FloorToInt(minXFloor);//the floor of pos.x
        int wyp = Mathf.FloorToInt(minYFloor);//the floor of pos.y
        int rowX = 0;//the row for the nodes array
        int columnY = 0;//the column for the nodes array
        float resultX = ((float)maxXFloor / Mathf.Abs(wxp));//for the next formula
        float resultY = ((float)maxYFloor / Mathf.Abs(wyp));//for the next formula
        float percentX = (pos.x + maxXFloor/ resultX) / maxXFloor;//the percent of pos.x from the grid itself
        float percentY = (pos.y + maxYFloor / resultY) / maxYFloor;//the percent of pos.y from the grid itself
        percentX = Mathf.Clamp01(percentX);//in case we not between 0 to 1
        percentY = Mathf.Clamp01(percentY);//in case we not between 0 to 1
        rowX = Mathf.RoundToInt((maxXFloor - 1) * percentX);//the row index for the node in pos.x
        columnY = Mathf.RoundToInt((maxYFloor - 1) * percentY);//the column index for the node in pos.y       
        WorldTile worldTileOfPos = sortedNodes[rowX, columnY];//the node itself      
        if(worldTileOfPos.walkable==false)//if the node is unwalkable then get the columnY-1 node
        {
            worldTileOfPos = sortedNodes[rowX, columnY-1];
        }
        
        return worldTileOfPos;
    }
    public int MaxSize//a method for getting the max size of the tilemap..for APathfinding class
    {
        get
        {
            return maxXFloor * maxYFloor;
        }
    }
    void createNodes()//a method to create the nodes
    {
        //use these to work out the size and where each node should be in the 2d array we'll use to store our nodes so we can work out neighbours and get paths
        int gridX = 0;//the place of the node in the array (1 dimension)
        int gridY = 0;//the place of the node in the array (2 dimension)
        bool foundTileOnLastPass = false;//a bool to know if we hace find a tile..its between the iteration of the loops
        //scan tiles and create nodes based on where they are
        for (int x = scanStartX; x < scanFinishX; x++)
        {
            for (int y = scanStartY; y < scanFinishY; y++)
            {
                //go through our world bounds in increments of 1
                TileBase tb = floor.GetTile(new Vector3Int(x, y, 0)); //check if we have a floor tile at that world coords
                if (tb == null)//if we dont then do nothing
                {

                }
                else//if we have then
                {
                    //if we do we go through the obstacle layers and check if there is also a tile at those coords if so we set founObstacle to true
                    bool foundObstacle = false;//a bool to know if an Obstacle is found
                    foreach (Tilemap t in obstacleLayers)//for each Tilemap in the obstacleLayers
                    {
                        TileBase tb2 = t.GetTile(new Vector3Int(x, y, 0));//get the TileBase in this tile           
                        if (tb2 == null)//if we dont have a floor tile then do nothing
                        {
                        }
                        else//if we have then
                        {
                            foundObstacle = true;//then we found an Obstacle
                        }
                    }
                    if (foundObstacle == false)//if we didnt find an Obstacle then
                    {
                        WorldTile wt = new WorldTile(true, new Vector3(x + 0.5f, y + 0.5f, 0), gridX, gridY);//create a walkable WorldTile(node) at the center of the tile with gridX and gridY
                        unSortedNodes.Add(wt);//add to unSortedNodes
                        foundTileOnLastPass = true;
                    }
                    else//if we did find an Obstacle then
                    {
                        WorldTile wt = new WorldTile(false, new Vector3(x + 0.5f, y + 0.5f, 0), gridX, gridY);//create a unwalkable WorldTile(node) at the center of the tile with gridX and gridY
                        unSortedNodes.Add(wt);//add to unSortedNodes
                        foundTileOnLastPass = true;
                    }
                    gridY++; //increment the y counter
                    if (gridX > gridBoundX)//if we are out of bound with X 
                    { 
                        gridBoundX = gridX;//fix it
                    }
                    if (gridY > gridBoundY)//if we are out of bound with Y
                    {
                        gridBoundY = gridY;//fix it
                    }
                }
            }
            if (foundTileOnLastPass == true)//since the grid is going from bottom to top on the Y axis on each iteration of the inside loop, if we have found tiles on this iteration we increment the gridX value and
            {
                //reset the y value
                gridX++;
                gridY = 0;
                foundTileOnLastPass = false;
            }
        }
        //put nodes into 2d array based on the
        sortedNodes = new WorldTile[gridBoundX + 1, gridBoundY + 1];//initialise the 2d array that will store our nodes in their position
        foreach (WorldTile wt in unSortedNodes)//for each WorldTile in unSortedNodes
        { 
            if ((wt.gridY == 0)||(wt.gridY == maxYFloor-1))//the first row and the last row of the map is unwalkable
            {
                wt.walkable = false;
            }
            sortedNodes[wt.gridX, wt.gridY] = wt;//put the WorldTile into the 2d array in the correct position
        }
        //assign neighbours to nodes
        for (int x = 0; x < gridBoundX; x++)//go through the 2d array and assign the neighbours of each node
        { 
            for (int y = 0; y < gridBoundY; y++)
            {
                if (sortedNodes[x, y] == null)//if null then do nothing
                { 

                }
                else//if they do then assign the neighbours
                {
                    WorldTile wt = sortedNodes[x, y]; 
                    wt.myNeighbours = GetNeighbours(wt);
                }
            }
        }
         WorldTilePositionFix();//call the method  WorldTilePositionFix
    }
    void OnDrawGizmos()//a method to show gizmo
    {
        if (sortedNodes != null && displayGridGizmos)
        {
            foreach (WorldTile n in sortedNodes)
            {
                if (n != null)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    //down here are spical gizmo...for test!
                    /*if(n.walkable==false)
                    {
                        foreach (WorldTile item in n.getMyNeighbours())
                        {
                            Gizmos.color = Color.blue;
                        }
                    }*/
                    /*
                    if (n == playerTile)
                    {
                        Gizmos.color = Color.blue;
                        //Debug.Log("tile: "+playerTile.gridPosition);
                        //Debug.Log("trans: "+GameObject.FindWithTag("Player").transform.position);
                    }
                    */
                    /*
                    if (n == startTile)
                    {
                        Gizmos.color = Color.yellow;
                        //Debug.Log("tile: "+playerTile.gridPosition);
                        //Debug.Log("trans: "+GameObject.FindWithTag("Player").transform.position);
                    }
                    */
                    Gizmos.DrawCube(new Vector3(n.gridPosition.x, n.gridPosition.y, 0), Vector3.one * (0.5f));
                }
            }
        }
    }
    public void  addEdgeNeighbours(WorldTile node)//a method to add edge dor the node..not the solution in the final game..level for test
    {
        List<WorldTile> neighbours = new List<WorldTile>();
        if (!((node.gridY > 0) || (node.gridY < maxYFloor - 1)) && (!node.walkable) )
        {
            WorldTile wt1 = new WorldTile(false, new Vector3(node.xMax, node.yMax, 0), node.gridX, node.gridY);//up right
            WorldTile wt2 = new WorldTile(false, new Vector3(node.xMin, node.yMin, 0), node.gridX, node.gridY);//down left
            WorldTile wt3 = new WorldTile(false, new Vector3(node.xMin, node.yMax, 0), node.gridX, node.gridY);//up left
            WorldTile wt4 = new WorldTile(false, new Vector3(node.xMax, node.yMin, 0), node.gridX, node.gridY);//down right
            neighbours = node.getMyNeighbours();
            foreach (WorldTile neighbour in neighbours)
            {
                if( ((neighbour.gridX== node.gridX+1)&&((neighbour.gridY == node.gridY +1)||(neighbour.gridY == node.gridY)))|| (neighbour.gridX == node.gridX) && (neighbour.gridY == node.gridY + 1))
                {
                    neighbour.myNeighbours.Add(wt1);
                }
                if (((neighbour.gridX == node.gridX + 1) && ((neighbour.gridY == node.gridY) || (neighbour.gridY == node.gridY-1))) || (neighbour.gridX == node.gridX) && (neighbour.gridY == node.gridY - 1))
                {
                    neighbour.myNeighbours.Add(wt4);
                }
                if (((neighbour.gridX == node.gridX - 1) && ((neighbour.gridY == node.gridY+1) || (neighbour.gridY == node.gridY))) || (neighbour.gridX == node.gridX) && (neighbour.gridY == node.gridY + 1))
                {
                    neighbour.myNeighbours.Add(wt3);
                }
                if (((neighbour.gridX == node.gridX - 1) && ((neighbour.gridY == node.gridY) || (neighbour.gridY == node.gridY-1))) || (neighbour.gridX == node.gridX) && (neighbour.gridY == node.gridY - 1))
                {
                    neighbour.myNeighbours.Add(wt2);
                }
                node.myNeighbours.Add(wt1);
                node.myNeighbours.Add(wt2);
                node.myNeighbours.Add(wt3);
                node.myNeighbours.Add(wt4);
            }
        }
    }
    public List<TileBase> getNeighbouringTiles(int x, int y, Tilemap t)//a method to add edge to the obstacle...not the solution in the final game..leve for tests
    {
        List<TileBase> retVal = new List<TileBase>();

        for (int i = x - unwalkableNodeBorder; i < x + unwalkableNodeBorder; i++)
        {
            for (int j = y - unwalkableNodeBorder; j < y + unwalkableNodeBorder; j++)
            {
                TileBase tile = t.GetTile(new Vector3Int(i, j, 0));
                if (tile == null)
                {

                }
                else
                {
                    retVal.Add(tile);
                }
            }
        }
        return retVal;
    }    
    public List<WorldTile> GetNeighbours(WorldTile node)//a method for assign the neighbors for each node
    {
        List<WorldTile> neighbours = new List<WorldTile>();//list for neighbors
        for (int x = -1; x <= 1; x++)//range of 1 in the X value
        {
            for (int y = -1; y <= 1; y++)//range of 1 in the Y value
            {
                //down here is not the solution in the final game..level for test
                //if ((x == 0 && y == 0) || (x == -1 && y == 1) || (x == -1 && y == -1) || (x == 1 && y == -1) || (x == -1 && y == -1))
                //this isthe way we do it
                if (x == 0 && y == 0)//if its the center the do continue 
                {
                    continue;
                }
                int checkX = node.gridX + x;//the X value for the neighbor
                int checkY = node.gridY + y;//the Y value for the neighbor
                if (checkX >= 0 && checkX < maxXFloor && checkY >= 0 && checkY < maxYFloor)//if its in the bound of the map
                {
                    if(sortedNodes[checkX, checkY]!=null)//if its not null
                        neighbours.Add(sortedNodes[checkX, checkY]);//add to the list
                }
            }
        }
        return neighbours;//return the complete list
    }
    public void WorldTilePositionFix()//a method to fix the obstacles..this is the solution in the final game
    {
        for (int i = 0; i < sortedNodes.GetLength(0); i++)
        {
            for (int j = 0; j < sortedNodes.GetLength(1); j++)
            {
                if (sortedNodes[i, j] != null)//if the node is not null then
                {
                    WorldTile currentNode = sortedNodes[i, j];//save this node
                    float currentNodeX = currentNode.gridPosition.x;//the x gridPosition
                    float currentNodeY = currentNode.gridPosition.y;//the Y gridPosition
                    float delta = 0.30f;//the space between the obstacle and the node
                    if (currentNode.walkable == true && currentNode.getMyNeighbours() != null)//is this node is walkable and it has neighbours then
                    {
                        foreach (WorldTile neighbor in currentNode.getMyNeighbours())//for each neighbour
                        {
                            if (neighbor != null)//if the neighbor is not null
                            {
                                if (neighbor.walkable == false)//if the neighbor is an obstacle
                                {
                                    //check the position if the obstacle from currentNode
                                    //and move currentNode to the opposite direction with delta
                                    if (neighbor.gridPosition.x > currentNode.gridPosition.x)
                                    {
                                        if (neighbor.gridPosition.y > currentNode.gridPosition.y)
                                        {
                                            currentNodeX -= delta;
                                            currentNodeY -= delta;
                                        }
                                        else if (neighbor.gridPosition.y == currentNode.gridPosition.y)
                                        {
                                            currentNodeX -= delta;
                                        }
                                        else if (neighbor.gridPosition.y < currentNode.gridPosition.y)
                                        {
                                            currentNodeX -= delta;
                                            currentNodeY += delta;
                                        }
                                    }
                                    else if (neighbor.gridPosition.x == currentNode.gridPosition.x)
                                    {
                                        if (neighbor.gridPosition.y > currentNode.gridPosition.y)
                                        {
                                            currentNodeY -= delta;
                                        }
                                        else if (neighbor.gridPosition.y < currentNode.gridPosition.y)
                                        {
                                            currentNodeY += delta;
                                        }
                                    }
                                    else if (neighbor.gridPosition.x < currentNode.gridPosition.x)
                                    {
                                        if (neighbor.gridPosition.y > currentNode.gridPosition.y)
                                        {
                                            currentNodeX += delta;
                                            currentNodeY -= delta;
                                        }
                                        else if (neighbor.gridPosition.y == currentNode.gridPosition.y)
                                        {
                                            currentNodeX += delta;
                                        }
                                        else if (neighbor.gridPosition.y < currentNode.gridPosition.y)
                                        {
                                            currentNodeX += delta;
                                            currentNodeY += delta;
                                        }
                                    }
                                }
                            }
                        }
                        //now check if the position of currentNode is not at the bound of the tile and if not then fix it
                        if (currentNodeX < currentNode.xMin)
                        {
                            currentNodeX = currentNode.xMin;
                        }
                        else if (currentNodeX > currentNode.xMax)
                        {
                            currentNodeX = currentNode.xMax;
                        }
                        if (currentNodeY < currentNode.yMin)
                        {
                            currentNodeY = currentNode.yMin;
                        }
                        else if (currentNodeY > currentNode.yMax)
                        {
                            currentNodeY = currentNode.yMax;
                        }
                        currentNode.gridPosition.x = currentNodeX;//change the node X gridPosition
                        currentNode.gridPosition.y = currentNodeY;//change the node Y gridPosition
                    }
                }
            }
        }      
    }
}