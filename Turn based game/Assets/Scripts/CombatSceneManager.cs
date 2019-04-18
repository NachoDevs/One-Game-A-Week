using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using SimpleJSON;

enum CombatFace
{
    PlayerSelect,
    PlayerAttack,
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
    public GameObject abilityButton;
    public GameObject abilitiesPanel;
    public GameObject characterSelectionArrow;

    [Header("Character")]
    public Transform charactersParent;
    public GameObject princeCharacter;
    public GameObject pirateCaptainCharacter;

    float[,] m_partyPos;

    CombatFace m_currFace;

    Camera m_cam;

    Character m_selectedCharacter;
    Character m_selectedEnemy;

    GameObject playerArrow;
    GameObject enemyArrow;

    RaycastHit2D m_hit;

    List<int> m_charactersHealth;
    List<GameObject> m_characters;
    List<GameObject> m_infoPanels;

    void Awake()
    {
        m_charactersHealth = new List<int>();
        m_characters = new List<GameObject>();
        m_infoPanels = new List<GameObject>(); ;
        m_cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadCharacters();
        LoadAbilities();
        HideAbilitiesPanel();
        m_currFace = CombatFace.PlayerSelect;
    }

    // Update is called once per frame
    void Update()
    {
        stateText.text = "CombatState:\n" + m_currFace.ToString();

        // this here will change as more characters can join the battle, this is not a good way to implement this
        m_charactersHealth.Clear();
        foreach (GameObject ch in m_characters)
        {
            m_charactersHealth.Add(ch.GetComponent<Character>().health);
        }

        for(int i = 0, j = 0; i < m_charactersHealth.Count + 1; ++i, j = 0)
        {
            foreach(GameObject iPanel in m_infoPanels)
            {
                if (i == j)
                {
                    iPanel.GetComponentInChildren<Slider>().value = m_charactersHealth[i];
                }
                ++j;
            }
        }
        // ---------

        HandleCombatState();
    }

    void StartTurn()
    {
        endTurnButton.interactable = true;
        m_currFace = CombatFace.PlayerSelect;
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
            partyAttackDamage.Add(c.GetComponent<Character>().attackDamageBoost);
        }

        SaveSystem.SaveGame(SaveSystem.GenerateGameData(partyHealth, partyAttackDamage, m_partyPos));

        SceneManager.LoadScene(0);
    }

    void HandleCombatState()
    {
        switch (m_currFace)
        {
            default:    // Playerface is the default state
            case CombatFace.PlayerSelect:
                if (Input.GetMouseButtonUp(0))
                {
                    m_hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    HideAbilitiesPanel();
                    try
                    {
                        if (m_hit.collider.gameObject.GetComponentInParent<Player>() != null)
                        {
                            m_selectedCharacter = m_hit.collider.gameObject.GetComponentInParent<Character>();
                            if(m_selectedCharacter.hasAttacked)
                            {
                                print("already attacked");
                                m_selectedCharacter = null;
                            }
                            else
                            {
                                m_currFace = CombatFace.PlayerAttack;
                                ShowAbilitiesPanel();
                                SelectCharacter(m_selectedCharacter);
                            }
                        }
                    }
                    catch (Exception e) { }
                }
                break;
            case CombatFace.PlayerAttack:
                if(Input.GetMouseButtonUp(0))
                {
                    m_hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    try
                    {
                        if (m_hit.collider.gameObject.GetComponentInParent<Enemy>() != null)
                        {
                            m_selectedEnemy = m_hit.collider.gameObject.GetComponentInParent<Character>();
                            SelectCharacter(m_selectedEnemy);
                        }
                        else if (m_hit.collider.gameObject.GetComponentInParent<Character>() == m_selectedCharacter)
                        {
                            UnselectCharacter(m_selectedCharacter);
                            m_currFace = CombatFace.PlayerSelect;
                        }
                    }
                    catch (Exception e) { }
                }
                break;
            case CombatFace.AIFace:
                print("AI face");
                StartTurn();
                break;
        }
    }

    void LoadCharacters()
    {
        GameObject prince = Instantiate(princeCharacter, new Vector3(-4f, 0f, -1f), new Quaternion(), charactersParent);
        m_characters.Add(prince);
        prince.transform.localScale = new Vector3(3, 3, 1);
        SetUpCharacterInfoPanel(prince.GetComponent<Character>().name, prince.GetComponent<Character>().health, (prince.GetComponent<Player>() == null) ? true : false);
        m_charactersHealth.Add(prince.GetComponent<Character>().health);


        GameObject captain = Instantiate(pirateCaptainCharacter, new Vector3(4f, 0f, -1f), new Quaternion(), charactersParent);
        m_characters.Add(captain);
        captain.transform.localScale = new Vector3(3, 3, 1);
        /* if he is an enemy then*/ captain.GetComponentInChildren<SpriteRenderer>().flipX = true;
        SetUpCharacterInfoPanel(captain.GetComponent<Character>().name, captain.GetComponent<Character>().health, (captain.GetComponent<Player>() == null) ? true : false);
        m_charactersHealth.Add(prince.GetComponent<Character>().health);

        foreach (GameObject c in m_characters)
        {
            c.GetComponent<CharacterMovement>().enabled = false;
        }

        GameData gd = SaveSystem.LoadGame();

        if (gd != null)
        {
            prince.GetComponent<Character>().health = gd.partyHealth[0];
            captain.GetComponent<Character>().health = gd.partyHealth[1];

            prince.GetComponent<Character>().attackDamageBoost = gd.partyAttackDamage[0];
            captain.GetComponent<Character>().attackDamageBoost = gd.partyAttackDamage[1];

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


    void SetUpCharacterInfoPanel(string t_characterName, int t_characterHealth, bool t_isEnemy)
    {
        GameObject panel = Instantiate((t_isEnemy)? enemyInfoPanel : characterInfoPanel
                                        , (t_isEnemy) ? enemyInfoPanelParent : characterInfoPanelParent);

        panel.GetComponentInChildren<TextMeshProUGUI>().text = t_characterName;
        panel.GetComponentInChildren<Slider>().value = t_characterHealth;

        m_infoPanels.Add(panel);
    }

    void ShowAbilitiesPanel()
    {
        abilitiesPanel.SetActive(true);

        foreach (Ability ability in m_selectedCharacter.abilities)
        {
            GameObject button = Instantiate(abilityButton, abilitiesPanel.transform);

            button.GetComponentInChildren<TextMeshProUGUI>().text = ability.type.ToString();

            button.GetComponent<Button>().onClick.AddListener(() => 
            {
                if(m_selectedEnemy == null)
                {
                    print("select and enemy!");
                }
                else
                {
                    m_selectedCharacter.UseAbility(ability.type, m_selectedEnemy);
                    m_selectedEnemy.health -= ability.damage;
                    m_currFace = CombatFace.PlayerSelect;
                    UnselectCharacter(m_selectedCharacter);
                    UnselectCharacter(m_selectedEnemy);
                }
            });
        }
    }

    void HideAbilitiesPanel()
    {
        abilitiesPanel.SetActive(false);

        foreach (Transform abilityButton in abilitiesPanel.transform)
        {
            Destroy(abilityButton.gameObject);
        }
    }

    void LoadAbilities()
    {
        string path = Application.dataPath + "/JSONs/characterAbilities.json";
        string jsonString = File.ReadAllText(path);

        JSONObject abilitiesJSON = JSON.Parse(jsonString) as JSONObject;

        foreach(GameObject character in m_characters)
        {
            Character charC = character.GetComponent<Character>();

            foreach (var characterJSON in abilitiesJSON)
            {
                if (characterJSON.Key.ToLower() != charC.name.ToLower())
                {
                    continue;
                }

                foreach (var ability in characterJSON.Value)
                {
                    Ability newAbility = new Ability();
                    newAbility.SetAbilityAs(ability.Value);
                    charC.abilities.Add(newAbility);
                }
            }

        }
    }

    void SelectCharacter(Character t_character)
    {
        GameObject arrow = Instantiate(characterSelectionArrow, t_character.gameObject.transform);
        arrow.transform.position = new Vector3(t_character.gameObject.transform.position.x
                                            , t_character.gameObject.transform.position.y + 1
                                            , t_character.gameObject.transform.position.z);
        arrow.transform.localScale /= 3;

        try
        {
            if(t_character.GetComponent<Player>() != null)
            {
                arrow.GetComponentInChildren<SpriteRenderer>().color = Color.green;
                playerArrow = arrow;
            }
            else
            {
                arrow.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                enemyArrow = arrow;
            }
        } catch(Exception e) { }
    }

    void UnselectCharacter(Character t_character)
    {
        try
        {
            if (t_character.GetComponent<Player>() != null)
            {
                m_selectedCharacter = null;
                Destroy(playerArrow);
            }
            else
            {
                m_selectedEnemy = null;
                Destroy(enemyArrow);
            }
        } catch(Exception e) { }
    }

}
