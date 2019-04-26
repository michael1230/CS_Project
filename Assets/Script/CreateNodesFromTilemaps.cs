using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class CreateNodesFromTilemaps : MonoBehaviour
{


    //public static CreateNodesFromTilemaps instance;//////////////////////////////////////////////////////////////////////////////////////

    //changed execution order for this and world builder
    public bool displayGridGizmos;//////////////////////////////////////////////////////////////////////////////////
    public Grid gridBase;
    public Tilemap floor;//floor of world
    private int maxXFloor;
    private int maxYFloor;
    private int minXFloor;
    private int minYFloor;
    public List<Tilemap> obstacleLayers; //all layers that contain objects to navigate around
    public int unwalkableNodeBorder = 1;
    //these are the bounds of where we are searching in the world for tiles, have to use world coords to check for tiles in the tile map
    public int scanStartX = -250, scanStartY = -250, scanFinishX = 250, scanFinishY = 250;


    //public List<GameObject> unsortedNodes;//all the nodes in the world
    //public GameObject[,] nodes; //sorted 2d array of nodes, may contain null entries if the map is of an odd shape e.g. gaps


    public List<WorldTile> wTYnsortedNodes;//all the nodes in the world
    public WorldTile[,] wTNodes;

    // Use this for initialization
    void Awake()
    {
        wTYnsortedNodes = new List<WorldTile>();
        maxXFloor = floor.size.x;//Map size
        maxYFloor = floor.size.y;
        minXFloor = floor.cellBounds.xMin;
        minYFloor = floor.cellBounds.yMin;
        generateNodes();

    }

    public void generateNodes()
    {

        createNodes();
        //just call this and plug the resulting 2d array of nodes into your own A* algorithm
    }

    // Update is called once per frame
    void Update()
    {
        //NodeFromPosition(PlayerController.instance.transform.position);////////////////////////////////
    }

    void Start()//////////////////////////////////////////////////////////////////////////////////////
    {
        //instance = this;
        //DontDestroyOnLoad(gameObject);

    }//////////////////////////////////////////////////////////////////////////////////////

    public int gridBoundX = 0, gridBoundY = 0;

    public WorldTile NodeFromPosition(Vector3 pos)//a method get the node from the position
    {
        //WorldTile firstWorldTile = wTNodes[0, 0];//the first node to get the min X and Y of the map
        int wxp = Mathf.FloorToInt(minXFloor);//the floor of pos.x
        int wyp = Mathf.FloorToInt(minYFloor);//the floor of pos.y
        int rowX = 0;//the row for the nodes array
        int columnY = 0;//the column for the nodes array
        //double resultX = ((double)maxXFloor / Mathf.Abs(wxp));//for the next formula
        //double resultY = ((double)maxYFloor / Mathf.Abs(wyp));//for the next formula
        //double percentX = (pos.x + maxXFloor / resultX) / maxXFloor;//the percent of pos.x from the grid itself
        //double percentY = (pos.y + maxYFloor / resultY) / maxYFloor;//the percent of pos.y from the grid itself
        //rowX = (int)Math.Round((maxXFloor - 1) * percentX);
        //columnY = (int)Math.Round((maxYFloor - 1) * percentY);//the column index for the node in pos.y
        float resultX = ((float)maxXFloor / Mathf.Abs(wxp));//for the next formula
        float resultY = ((float)maxYFloor / Mathf.Abs(wyp));//for the next formula
        float percentX = (pos.x + maxXFloor/ resultX) / maxXFloor;//the percent of pos.x from the grid itself
        float percentY = (pos.y + maxYFloor / resultY) / maxYFloor;//the percent of pos.y from the grid itself
        percentX = Mathf.Clamp01(percentX);//in case we not between 0 to 1
        percentY = Mathf.Clamp01(percentY);//in case we not between 0 to 1
        rowX = Mathf.RoundToInt((maxXFloor - 1) * percentX);//the row index for the node in pos.x
        columnY = Mathf.RoundToInt((maxYFloor - 1) * percentY);//the column index for the node in pos.y
        WorldTile worldTileOfPos = wTNodes[rowX, columnY];//the node itself
        return worldTileOfPos;
    }

    public int MaxSize////////////////////////////////////////////////////////////////////////////////
    {
        get
        {
            return maxXFloor * maxYFloor;
        }
    }


    void createNodes()
    {
        int gridX = 0; //use these to work out the size and where each node should be in the 2d array we'll use to store our nodes so we can work out neighbours and get paths
        int gridY = 0;
        bool foundTileOnLastPass = false;
        //scan tiles and create nodes based on where they are
        for (int x = scanStartX; x < scanFinishX; x++)
        {
            for (int y = scanStartY; y < scanFinishY; y++)
            {
                //go through our world bounds in increments of 1
                TileBase tb = floor.GetTile(new Vector3Int(x, y, 0)); //check if we have a floor tile at that world coords
                if (tb == null)
                {
                }
                else
                {
                    //if we do we go through the obstacle layers and check if there is also a tile at those coords if so we set founObstacle to true
                    bool foundObstacle = false;
                    foreach (Tilemap t in obstacleLayers)
                    {
                        TileBase tb2 = t.GetTile(new Vector3Int(x, y, 0));                    
                        if (tb2 == null)
                        {
                        }
                        else
                        {
                            foundObstacle = true;
                        }
                        //if we want to add an unwalkable edge round our unwalkable nodes then we use this to get the neighbours and make them unwalkable
                        /*if (unwalkableNodeBorder > 0)
                        {
                            List<TileBase> neighbours = getNeighbouringTiles(x, y, t);
                            foreach (TileBase tl in neighbours)
                            {
                                if (tl == null)
                                {
                                }
                                else
                                {
                                    foundObstacle = true;
                                }
                            }
                        }*/
                    }
                    if (foundObstacle == false)
                    {
                        //if we havent found an obstacle then we create a walkable node and assign its grid coords
                        WorldTile wt = new WorldTile(true, new Vector3(x + 0.5f, y + 0.5f, 0), gridX, gridY);
                        wTYnsortedNodes.Add(wt);
                        foundTileOnLastPass = true;
                    }
                    else
                    {
                        WorldTile wt = new WorldTile(false, new Vector3(x + 0.5f, y + 0.5f, 0), gridX, gridY);
                        wTYnsortedNodes.Add(wt);
                        foundTileOnLastPass = true;
                    }
                    gridY++; //increment the y counter
                    if (gridX > gridBoundX)
                    { //if the current gridX/gridY is higher than the existing then replace it with the new value
                        gridBoundX = gridX;
                    }

                    if (gridY > gridBoundY)
                    {
                        gridBoundY = gridY;
                    }
                }
            }
            if (foundTileOnLastPass == true)
            {//since the grid is going from bottom to top on the Y axis on each iteration of the inside loop, if we have found tiles on this iteration we increment the gridX value and
                //reset the y value
               // gridYmax = gridY;
                gridX++;
                gridY = 0;
                foundTileOnLastPass = false;
            }
        }
        //put nodes into 2d array based on the
        wTNodes = new WorldTile[gridBoundX + 1, gridBoundY + 1];//initialise the 2d array that will store our nodes in their position
        foreach (WorldTile wt in wTYnsortedNodes)
        { //go through the unsorted list of nodes and put them into the 2d array in the correct position
            if ((wt.gridY == 0)||(wt.gridY == maxYFloor-1))
            {
                wt.walkable = false;
            }
            wTNodes[wt.gridX, wt.gridY] = wt;

        }
        //assign neighbours to nodes
        for (int x = 0; x < gridBoundX; x++)
        { //go through the 2d array and assign the neighbours of each node
            for (int y = 0; y < gridBoundY; y++)
            {
                if (wTNodes[x, y] == null)
                { //check if the coords in the array contain a node
                }
                else
                {
                    WorldTile wt = wTNodes[x, y]; //if they do then assign the neighbours
                    wt.myNeighbours = GetNeighbours(wt);
                    //addEdgeNeighbours(wt);
                    //wt.myNeighbours.AddRange(GetNeighbours(wt));
                }
            }
        }
         WorldTilePositionFix();
        //after this we have our grid of nodes ready to be used by the astar algorigthm
    }
    //gets neighbours of a tile at x/y in a specific tilemap, can also have a border
    /*
        public List<WorldTile> path;//for test
        void OnDrawGizmos()
        {
            //Gizmos.DrawWireCube(new Vector3(3, 0, 0), new Vector3(maxXFloor, maxYFloor, 0));
            if (wTNodes != null && displayGridGizmos)
            {
                foreach (WorldTile n in wTNodes)
                {
                    if(n!=null)
                    {
                        Gizmos.color = (n.walkable) ? Color.white : Color.red;
                        if (path != null)
                            if (path.Contains(n))
                                Gizmos.color = Color.black;
                        Gizmos.DrawCube(new Vector3(n.gridPosition.x+0.5f, n.gridPosition.y+0.5f, 0), Vector3.one * (0.5f));
                    }

                }
            }
        }
        */
    void OnDrawGizmos()
    {
        //WorldTile playerTile = NodeFromPosition(GameObject.FindWithTag("Player").transform.position);
        //WorldTile startTile = NodeFromPosition(GameObject.FindWithTag("SmallMapEnemy").transform.position);
        if (wTNodes != null && displayGridGizmos)
        {
            foreach (WorldTile n in wTNodes)
            {
                if (n != null)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    /*if(n.walkable==false)
                    {
                        foreach (WorldTile item in n.getMyNeighbours())
                        {
                            Gizmos.color = Color.blue;
                        }
                    }*/
                    /*if (n == playerTile)
                    {
                        Gizmos.color = Color.blue;
                        //Debug.Log("tile: "+playerTile.gridPosition);
                        //Debug.Log("trans: "+GameObject.FindWithTag("Player").transform.position);
                    }
                    if (n == startTile)
                    {
                        Gizmos.color = Color.yellow;
                        //Debug.Log("tile: "+playerTile.gridPosition);
                        //Debug.Log("trans: "+GameObject.FindWithTag("Player").transform.position);
                    }*/

                    Gizmos.DrawCube(new Vector3(n.gridPosition.x, n.gridPosition.y, 0), Vector3.one * (0.5f));
                }
            }
        }
    }


    public void  addEdgeNeighbours(WorldTile node)
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

    public List<TileBase> getNeighbouringTiles(int x, int y, Tilemap t)
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

    //gets the neighbours of the coords passed in
    public List<WorldTile> GetNeighbours(WorldTile node)
    {
        List<WorldTile> neighbours = new List<WorldTile>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                //if ((x == 0 && y == 0) || (x == -1 && y == 1) || (x == -1 && y == -1) || (x == 1 && y == -1) || (x == -1 && y == -1))
                    continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < maxXFloor && checkY >= 0 && checkY < maxYFloor)
                {
                    if(wTNodes[checkX, checkY]!=null)
                        neighbours.Add(wTNodes[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }





    public void WorldTilePositionFix()
    {
        for (int i = 0; i < wTNodes.GetLength(0); i++)
        {
            for (int j = 0; j < wTNodes.GetLength(1); j++)
            {
                if (wTNodes[i, j] != null)
                {
                    WorldTile currentNode = wTNodes[i, j];
                    float currentNodeX = currentNode.gridPosition.x;
                    float currentNodeY = currentNode.gridPosition.y;
                    float delta = 0.45f;
                    if (currentNode.walkable == true && currentNode.getMyNeighbours() != null)
                    {
                        foreach (WorldTile node in currentNode.getMyNeighbours())
                        {
                            if (node != null)
                            {
                                if (node.walkable == false)
                                {
                                    if (node.gridPosition.x > currentNode.gridPosition.x)
                                    {
                                        if (node.gridPosition.y > currentNode.gridPosition.y)
                                        {
                                            currentNodeX -= delta;
                                            currentNodeY -= delta;
                                        }
                                        else if (node.gridPosition.y == currentNode.gridPosition.y)
                                        {
                                            currentNodeX -= delta;
                                        }
                                        else if (node.gridPosition.y < currentNode.gridPosition.y)
                                        {
                                            currentNodeX -= delta;
                                            currentNodeY += delta;
                                        }
                                    }
                                    else if (node.gridPosition.x == currentNode.gridPosition.x)
                                    {
                                        if (node.gridPosition.y > currentNode.gridPosition.y)
                                        {
                                            currentNodeY -= delta;
                                        }
                                        else if (node.gridPosition.y < currentNode.gridPosition.y)
                                        {
                                            currentNodeY += delta;
                                        }
                                    }
                                    else if (node.gridPosition.x < currentNode.gridPosition.x)
                                    {
                                        if (node.gridPosition.y > currentNode.gridPosition.y)
                                        {
                                            currentNodeX += delta;
                                            currentNodeY -= delta;
                                        }
                                        else if (node.gridPosition.y == currentNode.gridPosition.y)
                                        {
                                            currentNodeX += delta;
                                        }
                                        else if (node.gridPosition.y < currentNode.gridPosition.y)
                                        {
                                            currentNodeX += delta;
                                            currentNodeY += delta;
                                        }
                                    }
                                }
                            }
                        }
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
                        currentNode.gridPosition.x = currentNodeX;
                        currentNode.gridPosition.y = currentNodeY;
                    }
                }
            }
        }
    }
}