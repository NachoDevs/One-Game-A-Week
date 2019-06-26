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

    public List<Territory> neighbours;

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

        neighbours = new List<Territory>();

        StartCoroutine(NeighboursSetUp());
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

    IEnumerator NeighboursSetUp()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.convex = true;

        yield return new WaitForSeconds(1.01f);

        meshCollider.convex = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        if(collision.gameObject.GetComponent<Territory>() != null)
        {
            if (!neighbours.Contains(collision.gameObject.GetComponent<Territory>()))
            {
                neighbours.Add(collision.gameObject.GetComponent<Territory>());
            }
        }
    }
}
