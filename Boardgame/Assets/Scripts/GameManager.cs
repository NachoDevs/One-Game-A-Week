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

public enum Team
{
    Neutral,
    Blue,
    Red,
}

public class GameManager : MonoBehaviour
{
    public GameState currGameState;

    [Header("UI")]
    public GameObject TroopCountUI;
    public GameObject troopSliderPanel;
    public TextMeshProUGUI troopCount;

    [Space]

    [SerializeField] internal Color blueTerritoryColor;
    [SerializeField] internal Color redTerritoryColor;
    [SerializeField] internal Color defaultTerritoryColor;
    [SerializeField] internal Color hoveredTerritoryColor;
    [SerializeField] internal Color selectedTerritoryColor;

    Territory m_currTerritory;
    Territory m_prevTerritory;
    Territory m_selectedTerritory;

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

                        if (Input.GetMouseButtonUp(0))
                        {
                            if (m_currTerritory != null)
                            {
                                if(m_selectedTerritory != null)
                                {
                                    if(m_selectedTerritory.neighbours.Contains(m_currTerritory))
                                    {
                                        ManageCombat(m_selectedTerritory, m_currTerritory);

                                        m_selectedTerritory = null;
                                        troopSliderPanel.SetActive(false);
                                    }
                                }
                                else
                                {
                                    m_selectedTerritory = m_currTerritory;

                                    foreach (Territory t in m_currTerritory.neighbours)
                                    {
                                        t.UpdateTerritoryState(TerritoryState.Hovered);
                                    }

                                    ShowTroopSliderPanel();
                                }
                            }
                            else
                            {
                                m_selectedTerritory = null;
                                troopSliderPanel.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        m_currTerritory = null;
                    }

                    TileSelectedBehaviour();
                }
                catch (Exception e) { PrintException(e); }

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

    private void ManageCombat(Territory t_fromTerritory, Territory t_toTerritory)
    {
        int fromTroops = troopSliderPanel.GetComponent<TroopSliderPanel>().toSendTroops;

        if(fromTroops == 0)
        {
            return;
        }

        int casualties = (t_fromTerritory.team != t_toTerritory.team) ? fromTroops - t_toTerritory.troopCount : fromTroops;
        print(casualties);
        if(casualties > 0)
        {
            if(t_fromTerritory.team != t_toTerritory.team)
            {
                t_toTerritory.troopCount = 0;
            }

            t_toTerritory.team = t_fromTerritory.team;
            t_toTerritory.UpdateTerritoryState(TerritoryState.Default);
        }

        if (casualties == 0)
        {
            t_toTerritory.troopCount = 0;
        }
        else
        {
            t_toTerritory.troopCount += casualties;
        }

        t_fromTerritory.troopCount -= fromTroops;
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

    void ShowTroopSliderPanel()
    {
        troopSliderPanel.SetActive(true);
        TroopSliderPanel tsp = troopSliderPanel.GetComponent<TroopSliderPanel>();

        tsp.troopSlider.value = 0;
        tsp.maxTroops = m_selectedTerritory.troopCount;
        tsp.maxText.text = m_selectedTerritory.troopCount.ToString();
        tsp.troopSlider.maxValue = m_selectedTerritory.troopCount;
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
