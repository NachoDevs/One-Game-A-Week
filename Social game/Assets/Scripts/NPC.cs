using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using SimpleJSON;

public class NPC : MonoBehaviour
{
    public bool isTalking = false;
    public bool isHome = false;
    public bool waitingForFruit = false;

    public int friendshipLevel;

    public GameObject friendshipLevelPanel;
    public GameObject fullHeartUI;
    public GameObject halfHeartUI;
    public GameObject home;
    public GameObject statePanel;

    public FruitsEnum wantedFruit;

    public Personality personality;

    readonly int maxFriendshipLevel = 6;

    readonly float m_walkRadius = 5;

    [SerializeField]
    Animator m_stateAnimator;


    Dictionary<string, List<string>> m_speech;

    GameManager m_gm;

    List<GameObject> m_hearts;

    Vector3 m_targetPosition;
    Vector3 m_walkDirection;

    NavMeshAgent m_agent;

    RaycastHit m_hit;
        
    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_hearts = new List<GameObject>();

        LoadJSON();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("NPCMovement", .0f, 5);
        //friendshipLevel = 3;

        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        transform.position = home.GetComponent<House>().rallyPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTalking)
        {
            m_agent.SetDestination(transform.position);
        }
        else
        {
            float time = m_gm.GetComponent<DayNightController>().currentTimeOfDay;
            if (time < .2f || time > .8f)
            {
                m_agent.SetDestination(home.GetComponent<House>().rallyPosition.transform.position);
            }
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out m_hit))
        {
            if (m_hit.collider == GetComponentInChildren<CapsuleCollider>())
            {
                ManageFriendlyLevel();
                friendshipLevelPanel.SetActive(true);
            }
            else
            {
                friendshipLevelPanel.SetActive(true);
            }
        }
    }

    void NPCMovement()
    {
        m_walkDirection = UnityEngine.Random.insideUnitSphere * m_walkRadius;
        m_walkDirection += transform.position;
        NavMeshHit nmhit;
        NavMesh.SamplePosition(m_walkDirection, out nmhit, m_walkRadius, 1);
        m_targetPosition = nmhit.position;

        if (!isTalking)
        {
            m_agent.SetDestination(m_targetPosition);
        }
    }

    void ManageFriendlyLevel()
    {
        if (m_hearts.Count > 0)
        {
            foreach(GameObject heart in m_hearts)
            {
                Destroy(heart.gameObject);
            }
        }

        for (int i = 0; i < (friendshipLevel / 2); ++i)
        {
            m_hearts.Add(Instantiate(fullHeartUI, friendshipLevelPanel.transform));
        }

        if(friendshipLevel % 2 != 0)
        {
            m_hearts.Add(Instantiate(halfHeartUI, friendshipLevelPanel.transform));
        }
    }

    public void IncrementFriendshipLevel(int t_ammountToIncrement)
    {
        if(personality == Personality.loving)
        {
            t_ammountToIncrement = Mathf.Abs(t_ammountToIncrement);
        }

        if(friendshipLevel < maxFriendshipLevel)
        {
            friendshipLevel += t_ammountToIncrement;
        }

        m_stateAnimator.SetTrigger((t_ammountToIncrement > 0) ? "isInLove" : "isSad");
    }

    public void Talk(string t_sentenceType)
    {
        //m_stateAnimator.SetTrigger("isCrying");
        m_gm.m_speechBubble.AddToSay(m_speech[t_sentenceType][Random.Range(0, m_speech[t_sentenceType].Count)]);
    }

    public void Quest()
    {
        FruitsEnum randomFruit = (Random.Range(0, 2) > 0) ? FruitsEnum.apple: FruitsEnum.peach;

        wantedFruit = randomFruit;
        waitingForFruit = true;

        m_gm.m_speechBubble.AddToSay(m_speech["greets"][Random.Range(0, m_speech["greets"].Count)]);
        m_gm.m_speechBubble.AddToSay("Would you mind getting me a fruit? I would love a juicy " + randomFruit + ".");
    }

    private void LoadJSON()
    {

        string path = Application.dataPath + "/JSONs/speech.json";
        string jsonString = File.ReadAllText(path);

        JSONObject speechJSON = JSON.Parse(jsonString) as JSONObject;

        m_speech = new Dictionary<string, List<string>>();

        foreach(var personalityStuation in speechJSON)
        {
            if(personalityStuation.Key != personality.ToString())
            {
                continue;
            }

            foreach (var situationText in personalityStuation.Value)
            {
                m_speech.Add(situationText.Key, new List<string>());
                foreach (var text in situationText.Value)
                {
                    m_speech[situationText.Key].Add(text.Value);
                }
            }
        }

    }

}

public enum Personality
{
    funny,
    sad,
    loving,
    bored
}