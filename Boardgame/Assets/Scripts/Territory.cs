using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerritoryState
{
    Default,
    Hovered,
    Selected
}

public class Territory : MonoBehaviour
{
    public bool selected = false;

    static GameManager m_gm;

    Renderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();

        if (m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    }

    public void UpdateTerritoryState(TerritoryState t_state)
    {
        switch (t_state)
        {
            case TerritoryState.Hovered:
                m_renderer.material.SetColor("_Color", m_gm.hoveredTerritoryColor);
                break;
            case TerritoryState.Selected:
                if (selected)
                {
                    UpdateTerritoryState(TerritoryState.Default);
                }
                else
                {
                    m_renderer.material.SetColor("_Color", m_gm.selectedTerritoryColor);
                    selected = true;
                }
                break;
            default:
                m_renderer.material.SetColor("_Color", m_gm.defaultTerritoryColor);
                selected = false;
                break;
        }
    }
}
