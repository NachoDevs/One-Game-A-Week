using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum CombatFace
{
    PlayerFace,
    AIFace
}

public class CombatSceneManager : MonoBehaviour
{

    [Header("UI")]
    public Button endTurnButton;
    public TextMeshProUGUI stateText;
    public Transform characterInfoPanelParent;
    public Transform enemyInfoPanelParent;
    public GameObject characterInfoPanel;
    public GameObject enemyInfoPanel;

    [Header("Character")]
    public Transform charactersParent;
    public GameObject princeCharacter;
    public GameObject pirateCaptainCharacter;

    float[,] m_partyPos;

    CombatFace m_currFace;

    Camera m_cam;

    Character m_selectedCharacter;

    RaycastHit2D m_hit;

    List<GameObject> m_characters;

    void Awake()
    {
        m_characters = new List<GameObject>();
        m_cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadCharacters();
        m_currFace = CombatFace.PlayerFace;
    }

    // Update is called once per frame
    void Update()
    {
        stateText.text = "CombatState:\n" + m_currFace.ToString();

        HandleCombatState();
    }

    void StartTurn()
    {
        endTurnButton.interactable = true;
        m_currFace = CombatFace.PlayerFace;
    }

    public void EndTurn()
    {
        endTurnButton.interactable = false;
        m_currFace = CombatFace.AIFace;
    }

    public void LoadMainMap()
    {
        List<int> partyHealth = new List<int>();
        List<int> partyAttackDamage = new List<int>();
        foreach (GameObject c in m_characters)
        {
            partyHealth.Add(c.GetComponent<Character>().health);
            partyAttackDamage.Add(c.GetComponent<Character>().attackDamage);
        }

        SaveSystem.SaveGame(SaveSystem.GenerateGameData(partyHealth, partyAttackDamage, m_partyPos));

        SceneManager.LoadScene(0);
    }

    void HandleCombatState()
    {
        switch (m_currFace)
        {
            case CombatFace.PlayerFace:
                if (Input.GetMouseButtonUp(0))
                {
                    m_hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    try
                    {
                        if (m_hit.collider.gameObject.GetComponentInParent<Player>() != null)
                        {
                            m_selectedCharacter = m_hit.collider.gameObject.GetComponentInParent<Character>();
                        }
                    }
                    catch (Exception e) { }
                }
                break;
            case CombatFace.AIFace:
                print("AI face");
                StartTurn();
                break;
            default:
                break;
        }
    }

    void LoadCharacters()
    {
        GameObject prince = Instantiate(princeCharacter, new Vector3(-4f, -4f, -1f), new Quaternion(), charactersParent);
        m_characters.Add(prince);
        SetUpCharacterInfoPanel(prince.GetComponent<Character>().name, prince.GetComponent<Character>().health, (prince.GetComponent<Player>() == null) ? true : false);
        GameObject captain = Instantiate(pirateCaptainCharacter, new Vector3(4f, -4f, -1f), new Quaternion(), charactersParent);
        captain.GetComponentInChildren<SpriteRenderer>().flipX = true;
        m_characters.Add(captain);
        SetUpCharacterInfoPanel(captain.GetComponent<Character>().name, captain.GetComponent<Character>().health, (captain.GetComponent<Player>() == null) ? true : false);

        foreach (GameObject c in m_characters)
        {
            c.GetComponent<Character>().enabled = false;
        }

        GameData gd = SaveSystem.LoadGame();

        if (gd != null)
        {
            prince.GetComponent<Character>().health = gd.partyHealth[0];
            captain.GetComponent<Character>().health = gd.partyHealth[1];

            prince.GetComponent<Character>().attackDamage = gd.partyAttackDamage[0];
            captain.GetComponent<Character>().attackDamage = gd.partyAttackDamage[1];

            int charaterCount = gd.partyHealth.Length;
            int auxIndex = 0;
            m_partyPos = new float[2, charaterCount];

            foreach (GameObject c in m_characters)
            {
                m_partyPos[0, auxIndex] = gd.partyPositions[0, auxIndex];
                m_partyPos[1, auxIndex] = gd.partyPositions[1, auxIndex];

                ++auxIndex;
            }
        }
    }


    GameObject SetUpCharacterInfoPanel(string t_characterName, int t_characterHealth, bool t_isEnemy)
    {
        GameObject panel = Instantiate((t_isEnemy)? enemyInfoPanel : characterInfoPanel
                                        , (t_isEnemy) ? enemyInfoPanelParent : characterInfoPanelParent);

        panel.GetComponentInChildren<TextMeshProUGUI>().text = t_characterName;
        panel.GetComponentInChildren<Slider>().value = t_characterHealth;

        return panel;
    }

}
