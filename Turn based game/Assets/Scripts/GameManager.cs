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
    public GameObject princeCharacter;
    public GameObject elfCharacter;
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
            partyAttackDamage.Add(c.GetComponent<Character>().damageBoost);
        }

        int charaterCount = partyHealth.Count;
        float[,] partyPos = new float[2 , charaterCount];

        foreach (GameObject character in m_characters)
        {
            Character characterC = character.GetComponent<Character>();
            partyPos[0, characterC.characterIndex] = character.transform.position.x;
            partyPos[1, characterC.characterIndex] = character.transform.position.y;
        }

        SaveSystem.SaveGame(SaveSystem.GenerateGameData(partyHealth, partyAttackDamage, partyPos));
    }

    void LoadGame()
    {
        // Thinking on a better way...
        if(Character.id_prefab == null)
        {
            GameObject prince = Instantiate(princeCharacter, new Vector3(13.5f, 49.5f, -1f), new Quaternion());
            m_characters.Add(prince);

            GameObject elf = Instantiate(elfCharacter, new Vector3(13.5f, 48.5f, -1f), new Quaternion());
            m_characters.Add(elf);

            GameObject captain = Instantiate(pirateCaptainCharacter, new Vector3(15.5f, 51.5f, -1f), new Quaternion());
            m_characters.Add(captain);
        }
        else
        {
            foreach(GameObject character in Character.id_prefab.Values)
            {
                m_characters.Add(character);
            }
        }

        GameData gd = SaveSystem.LoadGame();

        if (gd != null)
        {
            foreach(GameObject character in m_characters)
            {
                character.GetComponent<CharacterMovement>().enabled = true;

                Character characterC = character.GetComponent<Character>();
                int characterIndex = characterC.characterIndex;
                character.transform.position = new Vector3(gd.partyPositions[0, characterIndex], gd.partyPositions[1, characterIndex], -1);
                character.transform.localScale = Vector3.one;
                characterC.health = gd.partyHealth[characterIndex];
                characterC.damageBoost = gd.partyDamageBoost[characterIndex];
            }
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
                    
                }catch (Exception e) { PrintException(e); }

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
                }catch(Exception e) { PrintException(e); }

                    break;
            case GameState.PlayerAttack:
                SaveGame();
                LoadCombatScene();
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

    void LoadCombatScene()
    {
        //foreach(GameObject character in m_characters)
        //{
        //    Destroy(character);
        //}
        Character.s_nextCharacterIndex = 0;


        SceneManager.LoadScene(1);
    }


    public static void PrintException(Exception t_e)
    {
        print
            (
            "---------Exception---------\n"
            + "Message: \t" + t_e.Message + "\n"
            + "Source: \t" + t_e.Source + "\n"
            + "Data: \t" + t_e.Data + "\n"
            + "Trace: \t" + t_e.StackTrace+ "\n"
            );
    }
}
