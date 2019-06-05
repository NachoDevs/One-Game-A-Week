using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public float gridSizeX = 1, gridSizeY = 1;

    public int gridBoundX = 0, gridBoundY = 0;
    public int unwalkableNodeBorder = 1;
    //these are the bounds of where we are searching in the world for tiles, have to use world coords to check for tiles in the tile map
    public int scanStartX = -250, scanStartY = -250, scanFinishX = 250, scanFinishY = 250;

    //changed execution order for this and world builder
    public Grid gridBase;
    public Tilemap floor;//floor of world
    public List<Tilemap> obstacleLayers; //all layers that contain objects to navigate around
    public GameObject nodePrefab;
    public Transform walkableNodesParent;
    public Transform nonWalkableNodesParent;

    public List<GameObject> unsortedNodes;//all the nodes in the world
    public GameObject[,] nodes; //sorted 2d array of nodes, may contain null entries if the map is of an odd shape e.g. gaps

    private void Awake()
    {
        unsortedNodes = new List<GameObject>();
        
        CreateNodes();
    }

    void CreateNodes()
    {
        int gridX = 0; //use these to work out the size and where each node should be in the 2d array we'll use to store our nodes so we can work out neighbours and get paths
        int gridY = 0;

        bool foundTileOnLastPass = false;

        //scan tiles and create nodes based on where they are
        for (int xCount = scanStartX; xCount < scanFinishX; xCount++)
        {
            float x = xCount * gridSizeX;

            for (int yCount = scanStartY; yCount < scanFinishY; yCount++)
            {
                float y = yCount * gridSizeY;

                //go through our world bounds in increments of 1
                TileBase tb = floor.GetTile(new Vector3Int(xCount, yCount, 0)); //check if we have a floor tile at that world coords
                if (tb == null)
                {
                }
                else
                {
                    //if we do we go through the obstacle layers and check if there is also a tile at those coords if so we set founObstacle to true
                    bool foundObstacle = false;
                    foreach (Tilemap t in obstacleLayers)
                    {
                        TileBase tb2 = t.GetTile(new Vector3Int(xCount, yCount, 0));

                        if (tb2 == null)
                        {

                        }
                        else
                        {
                            foundObstacle = true;
                        }

                        //if we want to add an unwalkable edge round our unwalkable nodes then we use this to get the neighbours and make them unwalkable
                        if (unwalkableNodeBorder > 0)
                        {
                            List<TileBase> neighbours = GetNeighbouringTiles(xCount, yCount, t);
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
                        }
                    }

                    if (foundObstacle == false)
                    {
                        //if we havent found an obstacle then we create a walkable node and assign its grid coords
                        GameObject node = (GameObject)Instantiate(nodePrefab, new Vector3(x + gridBase.transform.position.x + ((yCount % 2 != 0) ? .45f : 0), y + gridBase.transform.position.y, -.5f), Quaternion.Euler(0, 0, 0), walkableNodesParent);
                        WorldTile wt = node.GetComponent<WorldTile>();
                        wt.gridX = gridX;
                        wt.gridY = gridY;
                        foundTileOnLastPass = true; //say that we have found a tile so we know to increment the index counters
                        unsortedNodes.Add(node);

                        node.name = gridX.ToString() + " : " + gridY.ToString() + " - NODE";
                    }
                    else
                    {
                        //if we have found an obstacle then we do the same but make the node unwalkable
                        GameObject node = (GameObject)Instantiate(nodePrefab, new Vector3(x + gridBase.transform.position.x + ((yCount % 2 != 0) ? .45f : 0), y + gridBase.transform.position.y, -.5f), Quaternion.Euler(0, 0, 0), nonWalkableNodesParent);
                        //we add the gridBase position to ensure that the nodes are ontop of the tile they relate too
                        //node.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                        WorldTile wt = node.GetComponent<WorldTile>();
                        wt.gridX = gridX;
                        wt.gridY = gridY;
                        wt.walkable = false;
                        foundTileOnLastPass = true;
                        unsortedNodes.Add(node);
                        node.name = gridX.ToString() + " : " + gridY.ToString() + " - UNWALKABLE NODE";
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
                gridX++;
                gridY = 0;
                foundTileOnLastPass = false;
            }
        }

        //put nodes into 2d array based on the 
        nodes = new GameObject[gridBoundX + 1, gridBoundY + 1];//initialise the 2d array that will store our nodes in their position 
        foreach (GameObject g in unsortedNodes)
        { //go through the unsorted list of nodes and put them into the 2d array in the correct position
            WorldTile wt = g.GetComponent<WorldTile>();
            //Debug.Log (wt.gridX + " " + wt.gridY);
            nodes[wt.gridX, wt.gridY] = g;
        }

        //assign neighbours to nodes
        for (int x = 0; x < gridBoundX; x++)
        { //go through the 2d array and assign the neighbours of each node
            for (int y = 0; y < gridBoundY; y++)
            {
                if (nodes[x, y] == null)
                { //check if the coords in the array contain a node

                }
                else
                {
                    WorldTile wt = nodes[x, y].GetComponent<WorldTile>(); //if they do then assign the neighbours
                    //if (wt.walkable == true) {
                        wt.neighbours = GetNeighbours(x, y, gridBoundX, gridBoundY);
                    //}
                }
            }
        }
        //after this we have our grid of nodes ready to be used by the astar algorigthm
    }

    public List<TileBase> GetNeighbouringTiles(int x, int y, Tilemap t)
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
    public List<WorldTile> GetNeighbours(int x, int y, int width, int height)
    {

        List<WorldTile> myNeighbours = new List<WorldTile>();

        //needs the width & height to work out if a tile is not on the edge, also needs to check if the nodes is null due to the accounting for odd shapes


        if (x > 0 && x < width - 1)
        {
            //can get tiles on both left and right of the tile

            if (y > 0 && y < height - 1)
            {
                //top and bottom
                if (nodes[x + 1, y] == null)
                {

                }
                else
                {

                    WorldTile wt1 = nodes[x + 1, y].GetComponent<WorldTile>();
                    if (wt1 == null)
                    {
                    }
                    else
                    {
                        myNeighbours.Add(wt1);
                    }
                }

                if (nodes[x - 1, y] == null)
                {

                }
                else
                {
                    WorldTile wt2 = nodes[x - 1, y].GetComponent<WorldTile>();

                    if (wt2 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt2);

                    }
                }

                if (nodes[x, y + 1] == null)
                {

                }
                else
                {
                    WorldTile wt3 = nodes[x, y + 1].GetComponent<WorldTile>();
                    if (wt3 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt3);

                    }
                }

                if (nodes[x, y - 1] == null)
                {

                }
                else
                {

                    WorldTile wt4 = nodes[x, y - 1].GetComponent<WorldTile>();
                    if (wt4 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt4);
                    }
                }

            }
            else if (y == 0)
            {
                //just top
                if (nodes[x + 1, y] == null)
                {

                }
                else
                {

                    WorldTile wt1 = nodes[x + 1, y].GetComponent<WorldTile>();
                    if (wt1 == null)
                    {
                    }
                    else
                    {
                        myNeighbours.Add(wt1);
                    }
                }

                if (nodes[x - 1, y] == null)
                {

                }
                else
                {
                    WorldTile wt2 = nodes[x - 1, y].GetComponent<WorldTile>();

                    if (wt2 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt2);

                    }
                }
                if (nodes[x, y + 1] == null)
                {

                }
                else
                {
                    WorldTile wt3 = nodes[x, y + 1].GetComponent<WorldTile>();
                    if (wt3 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt3);

                    }
                }
            }
            else if (y == height - 1)
            {
                //just bottom
                if (nodes[x, y - 1] == null)
                {

                }
                else
                {

                    WorldTile wt4 = nodes[x, y - 1].GetComponent<WorldTile>();
                    if (wt4 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt4);
                    }
                }
                if (nodes[x + 1, y] == null)
                {

                }
                else
                {

                    WorldTile wt1 = nodes[x + 1, y].GetComponent<WorldTile>();
                    if (wt1 == null)
                    {
                    }
                    else
                    {
                        myNeighbours.Add(wt1);
                    }
                }

                if (nodes[x - 1, y] == null)
                {

                }
                else
                {
                    WorldTile wt2 = nodes[x - 1, y].GetComponent<WorldTile>();

                    if (wt2 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt2);

                    }
                }
            }


        }
        else if (x == 0)
        {
            //can't get tile on left
            if (y > 0 && y < height - 1)
            {
                //top and bottom

                if (nodes[x + 1, y] == null)
                {

                }
                else
                {

                    WorldTile wt1 = nodes[x + 1, y].GetComponent<WorldTile>();
                    if (wt1 == null)
                    {
                    }
                    else
                    {
                        myNeighbours.Add(wt1);
                    }
                }

                if (nodes[x, y - 1] == null)
                {

                }
                else
                {

                    WorldTile wt4 = nodes[x, y - 1].GetComponent<WorldTile>();
                    if (wt4 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt4);
                    }
                }
                if (nodes[x, y + 1] == null)
                {

                }
                else
                {
                    WorldTile wt3 = nodes[x, y + 1].GetComponent<WorldTile>();
                    if (wt3 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt3);

                    }
                }
            }
            else if (y == 0)
            {
                //just top
                if (nodes[x + 1, y] == null)
                {

                }
                else
                {

                    WorldTile wt1 = nodes[x + 1, y].GetComponent<WorldTile>();
                    if (wt1 == null)
                    {
                    }
                    else
                    {
                        myNeighbours.Add(wt1);
                    }
                }

                if (nodes[x, y + 1] == null)
                {

                }
                else
                {
                    WorldTile wt3 = nodes[x, y + 1].GetComponent<WorldTile>();
                    if (wt3 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt3);

                    }
                }
            }
            else if (y == height - 1)
            {
                //just bottom
                if (nodes[x + 1, y] == null)
                {

                }
                else
                {

                    WorldTile wt1 = nodes[x + 1, y].GetComponent<WorldTile>();
                    if (wt1 == null)
                    {
                    }
                    else
                    {
                        myNeighbours.Add(wt1);
                    }
                }
                if (nodes[x, y - 1] == null)
                {

                }
                else
                {

                    WorldTile wt4 = nodes[x, y - 1].GetComponent<WorldTile>();
                    if (wt4 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt4);
                    }
                }
            }
        }
        else if (x == width - 1)
        {
            //can't get tile on right
            if (y > 0 && y < height - 1)
            {
                //top and bottom
                if (nodes[x - 1, y] == null)
                {

                }
                else
                {
                    WorldTile wt2 = nodes[x - 1, y].GetComponent<WorldTile>();

                    if (wt2 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt2);

                    }
                }

                if (nodes[x, y + 1] == null)
                {

                }
                else
                {
                    WorldTile wt3 = nodes[x, y + 1].GetComponent<WorldTile>();
                    if (wt3 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt3);

                    }
                }
                if (nodes[x, y - 1] == null)
                {

                }
                else
                {

                    WorldTile wt4 = nodes[x, y - 1].GetComponent<WorldTile>();
                    if (wt4 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt4);
                    }
                }
            }
            else if (y == 0)
            {
                //just top
                if (nodes[x - 1, y] == null)
                {

                }
                else
                {
                    WorldTile wt2 = nodes[x - 1, y].GetComponent<WorldTile>();

                    if (wt2 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt2);

                    }
                }
                if (nodes[x, y + 1] == null)
                {

                }
                else
                {
                    WorldTile wt3 = nodes[x, y + 1].GetComponent<WorldTile>();
                    if (wt3 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt3);

                    }
                }
            }
            else if (y == height - 1)
            {
                //just bottom
                if (nodes[x - 1, y] == null)
                {

                }
                else
                {
                    WorldTile wt2 = nodes[x - 1, y].GetComponent<WorldTile>();

                    if (wt2 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt2);

                    }
                }
                if (nodes[x, y - 1] == null)
                {

                }
                else
                {

                    WorldTile wt4 = nodes[x, y - 1].GetComponent<WorldTile>();
                    if (wt4 == null)
                    {

                    }
                    else
                    {
                        myNeighbours.Add(wt4);
                    }
                }
            }
        }


        return myNeighbours;
    }

    public WorldTile GetTile(float t_x, float t_y)
    {
        foreach (var tile in unsortedNodes)
        {
            WorldTile wt = tile.GetComponent<WorldTile>();
            if (wt.walkable)
            {
                if (Mathf.Abs(wt.transform.position.x - t_x) < .5f && Mathf.Abs(wt.transform.position.y - t_y) < .5f)
                {
                    return wt;
                }
            }
        }
        return null;
    }

}
