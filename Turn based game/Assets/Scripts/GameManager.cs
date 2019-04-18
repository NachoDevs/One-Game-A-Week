using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
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

    [Header("UI")]
    public TextMeshProUGUI stateText;

    [Header("Character")]
    public Transform charactersParent;
    public GameObject princeCharacter;
    public GameObject pirateCaptainCharacter;

    List<GameObject> m_characters;

    PathfindingManager m_pfm;

    RaycastHit2D m_hit;

    Camera m_cam;

    Character m_selectedCharacter;

    WorldTile m_previousTile;
    WorldTile m_currentTile;

    void Awake()
    {
        m_pfm = GetComponent<PathfindingManager>();
        m_cam = Camera.main;
        m_characters = new List<GameObject>();

        m_previousTile = m_pfm.GetTile(0, 0);

        currGameState = GameState.PlayerSelectTile;

    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        HandleGameState();
    }

    public void SaveGame()
    {
        List<int> partyHealth = new List<int>();
        List<int> partyAttackDamage = new List<int>();
        foreach (GameObject c in m_characters)
        {
            partyHealth.Add(c.GetComponent<Character>().health);
            partyAttackDamage.Add(c.GetComponent<Character>().attackDamageBoost);
        }

        int charaterCount = partyHealth.Count;
        int auxIndex = 0;
        float[,] partyPos = new float[2 , charaterCount];

        foreach (GameObject c in m_characters)
        {
            partyPos[0, auxIndex] = c.transform.position.x;
            partyPos[1, auxIndex] = c.transform.position.y;

            ++auxIndex;
        }

        SaveSystem.SaveGame(SaveSystem.GenerateGameData(partyHealth, partyAttackDamage, partyPos));
    }

    void LoadGame()
    {
        GameObject prince = Instantiate(princeCharacter, new Vector3(13.5f, 49.5f, -1f), new Quaternion(), charactersParent);
        m_characters.Add(prince);
        GameObject captain = Instantiate(pirateCaptainCharacter, new Vector3(15.5f, 51.5f, -1f), new Quaternion(), charactersParent);
        m_characters.Add(captain);

        GameData gd = SaveSystem.LoadGame();

        if (gd != null)
        {
            prince.transform.position = new Vector3(gd.partyPositions[0, 0], gd.partyPositions[1, 0], -1);
            captain.transform.position = new Vector3(gd.partyPositions[0, 1], gd.partyPositions[1, 1], -1);

            prince.GetComponent<Character>().health = gd.partyHealth[0];
            captain.GetComponent<Character>().health = gd.partyHealth[1];

            prince.GetComponent<Character>().attackDamageBoost = gd.partyAttackDamage[0];
            captain.GetComponent<Character>().attackDamageBoost = gd.partyAttackDamage[1];
        }
    }

    void HandleGameState()
    {
        stateText.text = "Game State:\n" + currGameState.ToString();
        switch(currGameState)
        {
            default:    // PlayerSelectTile is the default state
            case GameState.PlayerSelectTile:
                m_hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                try
                {
                    m_currentTile = m_pfm.GetTile((int)m_hit.transform.position.x, (int)m_hit.transform.position.y);

                    if (m_hit.collider.gameObject.GetComponentInParent<Player>() != null)
                    {
                        m_selectedCharacter = m_hit.collider.gameObject.GetComponentInParent<Character>();
                        if (Input.GetMouseButtonUp(0))
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
                            if(m_hit.collider.gameObject.GetComponentInParent<Enemy>() != null)
                            {
                                currGameState = GameState.PlayerAttack;
                            }
                            else
                            {
                                m_selectedCharacter.MoveTo(m_currentTile);
                                currGameState = GameState.PlayerSelectTile;
                            }
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

                    }
                }catch(Exception e) { print(e.Message); }

                    break;
            case GameState.PlayerAttack:
                SaveGame();
                SceneManager.LoadScene(1);
                break;
            case GameState.AIturn:
                break;
            case GameState.GameOver:
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
