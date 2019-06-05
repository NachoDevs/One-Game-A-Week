using System;
using UnityEngine;

public enum GameState
{
    TileSelecting,
    Building,
}

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject buildingPrefab;

    [Header("Parents")]
    public Transform buildingParent;

    GameState m_currGameState;

    RaycastHit2D m_hit;

    Camera m_cam;

    GameObject m_selectedObject;
    GameObject m_hoveredObject;

    TileManager m_tman;

    WorldTile m_previousTile;
    WorldTile m_currentTile;

    void Awake()
    {
        m_cam = Camera.main;

        m_tman = GetComponent<TileManager>();

        m_currGameState = GameState.TileSelecting;
    }

    void Start()
    {
        m_previousTile = m_tman.GetTile(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        HandleGameState();
    }

    private void HandleGameState()
    {
        switch(m_currGameState)
        {
            case GameState.Building:
                TileSelectionBehaviour();

                if (Input.GetMouseButtonUp(0))
                {
                    GameObject building = Instantiate(buildingPrefab, m_currentTile.transform.position, Quaternion.identity, buildingParent);
                }

                break;
            default:
            case GameState.TileSelecting:
                TileSelectionBehaviour();
                break;
        }
    }

    void TileSelectionBehaviour()
    {
        try
        {
            m_hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            m_hoveredObject = m_hit.collider.transform.parent.gameObject;

            m_previousTile = m_currentTile;

            m_currentTile = m_tman.GetTile(m_hoveredObject.transform.position.x, m_hoveredObject.transform.position.y);

            if (!m_currentTile.selected)
            {
                m_currentTile.UpdateTIle(TileState.Hovered);
            }

            if (Input.GetMouseButtonUp(0))
            {
                m_selectedObject = m_hoveredObject;

                HandleSelection();
            }

            if (!m_previousTile.selected)
            {
                if (m_previousTile != m_currentTile)
                {
                    m_previousTile.UpdateTIle(TileState.Default);
                }
            }
        }
        catch (Exception e) { /*PrintException(e);*/ }
    }

    void HandleSelection()
    {
        if(m_selectedObject.GetComponent<WorldTile>() != null)
        {
            m_currentTile.UpdateTIle(TileState.Selected);

            // Only one tile can be selected
            foreach (GameObject wt in m_tman.unsortedNodes)
            {
                if (wt.GetComponent<WorldTile>() != m_currentTile)
                {
                    wt.GetComponent<WorldTile>().UpdateTIle(TileState.Default);
                }
            }

            return;
        }

        if (m_selectedObject.GetComponent<Building>() != null)
        {
            print("this is a building");

            return;
        }
    }

    public void GoToBuildingMode()
    {
        switch(m_currGameState)
        {
            default:
            case GameState.TileSelecting:
                m_currGameState = GameState.Building;
                break;
            case GameState.Building:
                m_currGameState = GameState.TileSelecting;
                break;
        }
    }

    public static void PrintException(Exception t_e)
    {
        print
            (
            "---------Exception---------\n"
            + "Message: \t" + t_e.Message + "\n"
            + "Source: \t" + t_e.Source + "\n"
            + "Data: \t" + t_e.Data + "\n"
            + "Trace: \t" + t_e.StackTrace + "\n"
            );
    }
}
