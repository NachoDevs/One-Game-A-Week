using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatSceneManager : MonoBehaviour
{

    [Header("Character")]
    public Transform charactersParent;
    public GameObject princeCharacter;
    public GameObject pirateCaptainCharacter;

    float[,] m_partyPos;

    List<GameObject> m_characters;

    void Awake()
    {
        m_characters = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadCharacters();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    void LoadCharacters()
    {
        GameObject prince = Instantiate(princeCharacter, new Vector3(-4f, -4f, -1f), new Quaternion(), charactersParent);
        m_characters.Add(prince);
        GameObject captain = Instantiate(pirateCaptainCharacter, new Vector3(4f, -4f, -1f), new Quaternion(), charactersParent);
        captain.GetComponentInChildren<SpriteRenderer>().flipX = true;
        m_characters.Add(captain);

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
}
