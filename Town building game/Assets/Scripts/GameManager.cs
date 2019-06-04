using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum GameState
{
    TileSelecting,
    Building,
}

public class GameManager : MonoBehaviour
{
    GameState m_currGameState;

    RaycastHit2D m_hit;

    Camera m_cam;

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
            default:
            case GameState.TileSelecting:
                try
                {
                    m_hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    m_previousTile = m_currentTile;

                    m_currentTile = m_tman.GetTile(m_hit.transform.position.x, m_hit.transform.position.y);

                    if(!m_currentTile.selected)
                    {
                        m_currentTile.UpdateTIle(TileState.Hovered);
                    }
                    //m_currentTile.hovered = true;

                    if (Input.GetMouseButtonUp(0))
                    {
                        m_currentTile.UpdateTIle(TileState.Selected);

                        // Only one tile can be selected
                        foreach (GameObject wt in m_tman.unsortedNodes)
                        {
                            if(wt.GetComponent<WorldTile>() != m_currentTile)
                            {
                                wt.GetComponent<WorldTile>().UpdateTIle(TileState.Default);
                            }
                        }
                    }

                    if(!m_previousTile.selected)
                    {
                        if(m_previousTile != m_currentTile)
                        {
                            m_previousTile.UpdateTIle(TileState.Default);
                        }
                        //m_currentTile.hovered = false;
                    }

                }
                catch (Exception e) { PrintException(e); }
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
