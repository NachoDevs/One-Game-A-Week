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
    NewTurn
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

    List<GameObject> m_party;
    List<GameObject> m_enemies;

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
        m_party = new List<GameObject>();
        m_enemies = new List<GameObject>();

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
        List<GameObject> characters = new List<GameObject>();
        characters.AddRange(m_party);
        characters.AddRange(m_enemies);

        SaveSystem.SaveGame(SaveSystem.GenerateGameData(characters, null));
    }

    public void EndTurn()
    {
        if(currGameState == GameState.AIturn)
        {
            currGameState = GameState.NewTurn;
        }
        else
        {
            currGameState = GameState.AIturn;
        }
    }

    void LoadGame()
    {
        // Thinking on a better way...
        if(Character.id_prefab == null)
        {
            GameObject prince = Instantiate(princeCharacter, new Vector3(13.5f, 49.5f, -1f), new Quaternion());
            prince.GetComponent<Character>().CheckIndex();
            m_party.Add(prince);

            GameObject elf = Instantiate(elfCharacter, new Vector3(13.5f, 48.5f, -1f), new Quaternion());
            elf.GetComponent<Character>().CheckIndex();
            m_party.Add(elf);

            GameObject captain = Instantiate(pirateCaptainCharacter, new Vector3(15.5f, 51.5f, -1f), new Quaternion());
            captain.GetComponent<Character>().CheckIndex();
            m_enemies.Add(captain);
        }
        else
        {
            foreach(GameObject character in Character.id_prefab.Values)
            {
                if(character.GetComponent<Player>() != null)
                {
                    m_party.Add(character);
                }
                else
                {
                    m_enemies.Add(character);
                }
            }
        }

        GameData gd = SaveSystem.LoadGame();

        if (gd != null)
        {
            List<GameObject> characters = new List<GameObject>();
            characters.AddRange(m_party);
            characters.AddRange(m_enemies);
            foreach (GameObject character in characters)
            {
                character.GetComponent<CharacterMovement>().enabled = true;

                Character characterC = character.GetComponent<Character>();
                int characterIndex = characterC.characterIndex;
                character.transform.position = new Vector3(gd.charsPositions[0, characterIndex], gd.charsPositions[1, characterIndex], -1);
                character.transform.localScale = Vector3.one;
                characterC.health = gd.charsHealth[characterIndex];
                characterC.damageBoost = gd.charsDamageBoost[characterIndex];
                characterC.isDead = gd.charsDead[characterIndex];
                characterC.hasMoved = gd.charsHaveMoved[characterIndex];

                if (characterC.isDead)
                {
                    characterC.Die();
                }
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
                            if(!m_selectedCharacter.hasMoved)
                            {
                                CharacterSelectedBehaviour();
                                //break;
                            }
                            else
                            {
                                print("Already moved");
                            }
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
                            m_selectedCharacter.hasMoved = true;

                            if(m_hit.collider.gameObject.GetComponentInParent<Enemy>() != null)
                            {
                                if(m_hit.collider.gameObject.GetComponentInParent<Character>().isDead)
                                {
                                    m_selectedCharacter.MoveTo(m_currentTile);
                                    currGameState = GameState.PlayerSelectTile;
                                }
                                else
                                {
                                    currGameState = GameState.PlayerAttack;
                                }
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
                print("AI turn");
                EndTurn();
                break;
            case GameState.NewTurn:
                ResetTurn();
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

    void ResetTurn()
    {
        foreach (GameObject character in m_party)
        {
            character.GetComponent<Character>().NewTurn();
        }

        currGameState = GameState.PlayerSelectTile;
    }

    void LoadCombatScene()
    {
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
