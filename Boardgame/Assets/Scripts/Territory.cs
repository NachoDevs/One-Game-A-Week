using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public int troopCount = 0;

    public List<Territory> neighbours;

    static GameManager m_gm;

    TextMeshProUGUI troopCountText;

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
        SetUpCanvas();
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

        yield return new WaitForSeconds(.01f);

        meshCollider.convex = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void SetUpCanvas()
    {
        //GameObject newCanvas = new GameObject("Canvas");
        //newCanvas.transform.SetParent(transform, true);
        //newCanvas.transform.localScale.Scale(new Vector3(.0005f, .0005f, .0005f));
        //newCanvas.AddComponent<CanvasRenderer>();
        //Canvas c = newCanvas.AddComponent<Canvas>();
        //c.renderMode = RenderMode.WorldSpace;

        //GameObject panel = new GameObject("Panel");
        //panel.transform.localScale.Scale(new Vector3(.001f, .001f, .001f));
        //panel.AddComponent<CanvasRenderer>();
        //troopCountText = panel.AddComponent<TextMeshProUGUI>();
        //troopCountText.color = Color.black;
        //troopCountText.text = troopCount.ToString();

        //panel.transform.SetParent(newCanvas.transform, false);

        //--------------

        //GameObject troopPanel = Instantiate(m_gm.TroopCountUI);
        //troopPanel.transform.SetParent(transform, false);
        //troopPanel.transform.position = transform.position + new Vector3(12.5f, 10, 514f);
        //troopCountText = troopPanel.GetComponentInChildren<TextMeshProUGUI>();
        //troopCountText.text = troopCount.ToString();


    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Territory>() != null)
        {
            if (!neighbours.Contains(collision.gameObject.GetComponent<Territory>()))
            {
                neighbours.Add(collision.gameObject.GetComponent<Territory>());
            }
        }
    }
}
