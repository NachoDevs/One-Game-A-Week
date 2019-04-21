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
    public float maxCombatJoinRange = 3.5f;

    public GameState currGameState;

    [Header("UI")]
    public TextMeshProUGUI stateText;

    [Header("Character")]
    public GameObject princeCharacter;
    public GameObject elfCharacter;
    public GameObject pirateCaptainCharacter;
    public GameObject pirate1Character;
    public GameObject pirate2Character;

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

        bool endGame = true;
        foreach (GameObject character in m_enemies)
        {
            if (!character.GetComponent<Character>().isDead)
            {
                endGame = false;
                break;
            }
        }

        foreach (GameObject character in m_party)
        {
            if (!character.GetComponent<Character>().isDead)
            {
                endGame = false;
                break;
            }
        }

        if (endGame)
        {
            Application.Quit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleGameState();
    }

    public void SaveButton()
    {
        SaveGame(null);
    }

    void SaveGame(List<int> t_charactersInvolved)
    {
        List<GameObject> characters = new List<GameObject>();
        characters.AddRange(m_party);
        characters.AddRange(m_enemies);

        SaveSystem.SaveGame(SaveSystem.GenerateGameData(characters, t_charactersInvolved, null));
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

            GameObject captain1 = Instantiate(pirateCaptainCharacter, new Vector3(15.5f, 51.5f, -1f), new Quaternion());
            captain1.GetComponent<Character>().CheckIndex();
            m_enemies.Add(captain1);

            GameObject pirate1 = Instantiate(pirate2Character, new Vector3(24.5f, 54.5f, -1f), new Quaternion());
            pirate1.GetComponent<Character>().CheckIndex();
            m_enemies.Add(pirate1);

            GameObject pirate2 = Instantiate(pirate2Character, new Vector3(5.5f, 65.5f, -1f), new Quaternion());
            pirate2.GetComponent<Character>().CheckIndex();
            m_enemies.Add(pirate2);

            GameObject pirate3 = Instantiate(pirate2Character, new Vector3(12.5f, 60.5f, -1f), new Quaternion());
            pirate3.GetComponent<Character>().CheckIndex();
            m_enemies.Add(pirate3);

            GameObject pirate4 = Instantiate(pirate1Character, new Vector3(4.5f, 63.5f), new Quaternion());
            pirate4.GetComponent<Character>().CheckIndex();
            m_enemies.Add(pirate4);

            GameObject captain2 = Instantiate(pirateCaptainCharacter, new Vector3(14.5f, 62.5f, -1f), new Quaternion());
            captain2.GetComponent<Character>().CheckIndex();
            m_enemies.Add(captain2);

            GameObject pirate6 = Instantiate(pirate2Character, new Vector3(15.5f, 63.5f, -1f), new Quaternion());
            pirate6.GetComponent<Character>().CheckIndex();
            m_enemies.Add(pirate6);

            GameObject pirate7 = Instantiate(pirate1Character, new Vector3(13.5f, 63.5f, -1f), new Quaternion());
            pirate7.GetComponent<Character>().CheckIndex();
            m_enemies.Add(pirate7);

            GameObject pirate8 = Instantiate(pirate2Character, new Vector3(8.5f, 56.5f, -1f), new Quaternion());
            pirate8.GetComponent<Character>().CheckIndex();
            m_enemies.Add(pirate8);

            GameObject pirate9 = Instantiate(pirate1Character, new Vector3(11.5f, 55.5f, -1f), new Quaternion());
            pirate9.GetComponent<Character>().CheckIndex();
            m_enemies.Add(pirate9);

            GameObject pirate10 = Instantiate(pirate2Character, new Vector3(20.5f, 59.5f, -1f), new Quaternion());
            pirate10.GetComponent<Character>().CheckIndex();
            m_enemies.Add(pirate10);

            GameObject pirate11 = Instantiate(pirate1Character, new Vector3(19.5f, 59.5f, -1f), new Quaternion());
            pirate11.GetComponent<Character>().CheckIndex();
            m_enemies.Add(pirate11);
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

        m_cam.GetComponentInParent<CameraScript>().StartCameraPosition(m_party[0].transform.position, m_party[1].transform.position);
    }

    void HandleGameState()
    {
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
                                StartCoroutine(PrintText(stateText, "This unit has already moved!"));
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
                LoadCombatScene(GetInvolvedCharacters(m_selectedCharacter.transform.position));
                break;
            case GameState.AIturn:
                HandleAI();
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

    void HandleAI()
    {
        foreach (GameObject enemy in m_enemies)
        {
            if(enemy.GetComponent<Character>().isDead)
            {
                continue;
            }

            WorldTile enemyTile = m_pfm.GetTile((int)enemy.transform.position.x, (int)enemy.transform.position.y);
            List<WorldTile> neighbours = enemyTile.myNeighbours;

            bool badTile = true;
            WorldTile target = null;
            while (badTile)
            {
                target = neighbours[UnityEngine.Random.Range(0, neighbours.Count)];
                if(target.walkable)
                {
                    badTile = false;
                }
            }

            float closest = float.MaxValue;
            GameObject targetChar = null;
            foreach (GameObject character in m_party)
            {
                float dist = Vector3.Distance(enemy.transform.position, character.transform.position);
                if (dist < closest)
                {
                    closest = dist;
                    targetChar = character;
                }
            }

            if (closest < enemy.GetComponent<Character>().targettingRange)
            {
                if (targetChar != null)
                {
                    target = Pathfinding.FindPath(enemyTile, m_pfm.GetTile((int)targetChar.transform.position.x, (int)targetChar.transform.position.y))[0];
                }
            }

            foreach (GameObject character in m_party)
            {
                if(m_pfm.GetTile((int)targetChar.transform.position.x, (int)targetChar.transform.position.y) == target)
                {
                    LoadCombatScene(GetInvolvedCharacters(character.transform.position));
                    break;
                }
            }

            enemy.GetComponent<Character>().MoveTo(target);
        }
    }

    List<int> GetInvolvedCharacters(Vector3 t_combatCenterPoint)
    {
        List<int> joining = new List<int>();
        List<GameObject> characters = new List<GameObject>();
        characters.AddRange(m_party);
        characters.AddRange(m_enemies);
        foreach (GameObject character in characters)
        {
            if(Vector3.Distance(t_combatCenterPoint, character.transform.position) < maxCombatJoinRange)
            {
                joining.Add(character.GetComponent<Character>().characterIndex);
            }
        }

        return joining;
    }

    void ResetTurn()
    {
        foreach (GameObject character in m_party)
        {
            character.GetComponent<Character>().NewTurn();
        }

        currGameState = GameState.PlayerSelectTile;
    }

    void LoadCombatScene(List<int> t_charactersInvolved)
    {
        SaveGame(t_charactersInvolved);

        Character.s_nextCharacterIndex = 0;

        SceneManager.LoadScene(1);
    }

    public static IEnumerator PrintText(TextMeshProUGUI t_textBox, string t_text)
    {
        t_textBox.text = t_text;
        yield return new WaitForSeconds(3);
        t_textBox.text = "";
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
