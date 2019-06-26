﻿using System;
using System.Collections;
using System.Collections.Generic;
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

                //if (Input.GetMouseButtonUp(0))
                //{
                //    if(m_currTerritory != null)
                //    {
                //        UpdateTerritoryColor(m_currTerritory, selectedTerritoryColor);
                //    }
                //}
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
        //if (m_currTerritory != null)
        //{
        //    if(m_currTerritory.GetComponent<Renderer>().material.GetColor("_Color") != selectedTerritoryColor)
        //    {
        //        m_currTerritory.GetComponent<Territory>().UpdateTerritoryState(TerritoryState.Hovered);
        //    }

        //    if (m_prevTerritory != null)
        //    {
        //        if (m_prevTerritory != m_currTerritory)
        //        {
        //            UpdateTerritoryColor(m_prevTerritory, defaultTerritoryColor);
        //        }
        //    }

        //    m_prevTerritory = m_currTerritory;
        //}

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

    void UpdateTerritoryColor(GameObject t_territory, Color t_color)
    {
        t_territory.GetComponent<Renderer>().material.SetColor("_Color", t_color);
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
