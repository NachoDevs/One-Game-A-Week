using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState
{
    PlayerSelectTile,
    PlayerMove,
    PlayerAttack,
    AIturn,
    NewTurn
}

public class GameManager : MonoBehaviour
{
    public GameState currGameState;

    [Header("UI")]
    public GameObject TroopCountUI;
    public TextMeshProUGUI troopCount;

    [Space]

    [SerializeField] internal Color defaultTerritoryColor;
    [SerializeField] internal Color hoveredTerritoryColor;
    [SerializeField] internal Color selectedTerritoryColor;

    Territory m_currTerritory;
    Territory m_prevTerritory;

    Camera m_cam;

    RaycastHit m_hit;

    // Start is called before the first frame update
    void Start()
    {
        m_cam = Camera.main;

        currGameState = GameState.PlayerSelectTile;
    }

    // Update is called once per frame
    void Update()
    {
        HandleGameState();

        if(m_currTerritory != null)
        {
            troopCount.text = m_currTerritory.troopCount.ToString();
        }
    }

    void HandleGameState()
    {
        switch (currGameState)
        {
            default:    // PlayerSelectTile is the default state
            case GameState.PlayerSelectTile:

                try
                {
                    Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out m_hit, Mathf.Infinity))
                    {
                        m_currTerritory = null;
                        if (m_hit.collider.gameObject.GetComponent<Territory>() != null)
                        {
                            m_currTerritory = m_hit.collider.gameObject.GetComponent<Territory>();
                        }
                    }
                    TileSelectedBehaviour();
                }
                catch (Exception e) { PrintException(e); }

                if (Input.GetMouseButtonUp(0))
                {
                    if (m_currTerritory != null)
                    {
                        foreach(Territory t in m_currTerritory.neighbours)
                        {
                            t.UpdateTerritoryState(TerritoryState.Selected);
                        }
                    }
                }
                break;
            case GameState.PlayerMove:
                break;
            case GameState.PlayerAttack:
                break;
            case GameState.AIturn:
                //HandleAI();
                //EndTurn();
                break;
            case GameState.NewTurn:
                //ResetTurn();
                break;
        }
    }

    void TileSelectedBehaviour()
    {
        if (m_currTerritory != null)
        {
            if(m_prevTerritory != null)
            {
                if (m_prevTerritory != m_currTerritory)
                {
                    if (!m_prevTerritory.selected)
                    {
                        m_prevTerritory.UpdateTerritoryState(TerritoryState.Default);
                    }
                }
            }

            if (!m_currTerritory.selected)
            {
                m_currTerritory.UpdateTerritoryState(TerritoryState.Hovered);
            }

            m_prevTerritory = m_currTerritory;
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
