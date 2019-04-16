using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : MonoBehaviour
{
    public bool walkable = true;

    public int gCost;               // Distance from the starting node
    public int hCost;               // Distance from the goal node
    public int gridX, gridY;        // Location on grid

    public List<WorldTile> myNeighbours;

    public WorldTile parent;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
