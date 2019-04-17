using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using System;

public enum GameState
{
    PlayerSelectTile,
    PlayerMove,
    PlayerAttack,
    AIturn,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public GameState currGameState;

    public TextMeshProUGUI stateText;

    PathfindingManager m_pfm;

    RaycastHit2D m_hit;

    Camera m_cam;

    Character m_selectedCharacter;

    WorldTile m_previousTile;
    WorldTile m_currentTile;

    // Start is called before the first frame update
    void Start()
    {
        m_pfm = GetComponent<PathfindingManager>();
        m_cam = Camera.main;

        m_previousTile = m_pfm.GetTile(0, 0);

        currGameState = GameState.PlayerSelectTile;
    }

    // Update is called once per frame
    void Update()
    {
        HandleGameState();
    }
    
    void HandleGameState()
    {
        stateText.text = "Game State:\n" + currGameState.ToString();
        switch(currGameState)
        {
            case GameState.PlayerSelectTile:
                m_hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                try
                {
                    m_currentTile = m_pfm.GetTile((int)m_hit.transform.position.x, (int)m_hit.transform.position.y);

                    if ((m_selectedCharacter = m_hit.collider.gameObject.GetComponentInParent<Character>()) != null)
                    {
                        if(Input.GetMouseButtonUp(0))
                        {
                            CharacterSelectedBehaviour();
                            //break;
                        }
                    }

                    if (m_currentTile != null)
                    {
                        TileSelectedBehaviour();
                        //break;
                    }         
                    
                }catch (Exception e) { print(e.Message); }

                break;
            case GameState.PlayerMove:
                
                try
                {
                    if(Input.GetMouseButtonUp(0))
                    {
                        m_hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                        m_previousTile = m_currentTile;

                        m_currentTile = m_pfm.GetTile((int)m_hit.transform.position.x, (int)m_hit.transform.position.y);

                        if(m_currentTile.selected)
                        {
                            m_selectedCharacter.MoveTo(m_currentTile);
                        }

                        if(m_currentTile != m_previousTile)
                        {
                            foreach (WorldTile wt in m_previousTile.myNeighbours)
                            {
                                if (wt.selected)
                                {
                                    wt.UpdateTIle(TileState.Default);
                                }
                            }
                        }

                        currGameState = GameState.PlayerSelectTile;
                    }
                }catch(Exception e) { print(e.Message); }

                    break;
            case GameState.PlayerAttack:
                break;
            case GameState.AIturn:
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }
    }

    void TileSelectedBehaviour()
    {
        if (m_currentTile != null)
        {
            if (m_previousTile != m_currentTile)
            {
                if (!m_previousTile.selected)
                {
                    m_previousTile.UpdateTIle(TileState.Default);
                }
            }

            if (!m_currentTile.selected)
            {
                m_currentTile.UpdateTIle(TileState.Hovered);
            }

            /*if (Input.GetMouseButtonUp(0))
            {
                m_currentTile.UpdateTIle(TileState.Selected);
            }*/

            m_previousTile = m_currentTile;
        }
    }

    void CharacterSelectedBehaviour()
    {
        currGameState = GameState.PlayerMove;

        // Change for "in walking range" tiles. Do it on the character script?
        foreach (WorldTile wt in m_currentTile.myNeighbours)
        {
            if (!wt.selected)
            {
                wt.UpdateTIle(TileState.Selected);
            }
        }
    }
}
