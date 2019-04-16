using UnityEngine;
using System.Collections.Generic;

public static class Pathfinding
{
    static List<WorldTile> s_path;

    public static List<WorldTile> FindPath(WorldTile t_startNode, WorldTile t_targetNode)
    {
        s_path = new List<WorldTile>();   // Path reset

        List<WorldTile> openSet = new List<WorldTile>();
        HashSet<WorldTile> closedSet = new HashSet<WorldTile>();
        openSet.Add(t_startNode);

        while (openSet.Count > 0)
        {
            WorldTile node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == t_targetNode)
            {
                RetracePath(t_startNode, t_targetNode);
                return s_path;
            }

            foreach (WorldTile neighbour in node.myNeighbours)
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, t_targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }

    static void RetracePath(WorldTile startNode, WorldTile endNode)
    {
        WorldTile currentNode = endNode;

        while (currentNode != startNode)
        {
            s_path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        s_path.Reverse();
    }

    static int GetDistance(WorldTile nodeA, WorldTile nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
