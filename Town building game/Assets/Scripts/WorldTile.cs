using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState
{
    Default,
    Hovered,
    Selected
}

public class WorldTile : MonoBehaviour
{
    public bool walkable = true;
    public bool selected = false;

    public int gridX, gridY;        // Location on grid

    public List<WorldTile> myNeighbours;

    public WorldTile parent;

    SpriteRenderer m_sp;

    void Start()
    {
        m_sp = GetComponentInChildren<SpriteRenderer>();
    }

    public void UpdateTIle(TileState t_state)
    {
        switch (t_state)
        {
            case TileState.Hovered:
                m_sp.color = new Color(1, 0, 0, .5f);
                break;
            case TileState.Selected:
                if (selected)
                {
                    UpdateTIle(TileState.Default);
                }
                else
                {
                    m_sp.color = new Color(1, 0, 1, 1);
                    selected = true;
                }
                break;
            default:
                m_sp.color = new Color(0, 0, 0, .05f);
                selected = false;
                break;
        }
    }
}
