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
    public TileState tileState;

    public bool walkable = true;
    public bool selected = false;
    //public bool hovered = false;

    public int gridX, gridY;        // Location on grid

    public List<WorldTile> neighbours;

    public WorldTile parent;

    SpriteRenderer m_sp;

    void Start()
    {
        m_sp = GetComponentInChildren<SpriteRenderer>();
        UpdateTIle(TileState.Default);
    }

    public void UpdateTIle(TileState t_state)
    {
        tileState = t_state;

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
                //if(!hovered)
                //{
                    m_sp.color = new Color(0, 0, 0, 0);
                //}
                selected = false;
                break;
        }
    }
}
