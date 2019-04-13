using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using SimpleJSON;

public class NPC : MonoBehaviour
{
    public bool m_isTalking = false;

    public int friendshipLevel;

    public GameObject friendshipLevelPanel;
    public GameObject statePanel;

    public GameObject fullHeartUI;
    public GameObject halfHeartUI;

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


    //////////////////////////

    List<Transform> interestPoints;
    List<GameObject> preferedItems;

    /////////////////////////
        
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out m_hit))
        {
            if (m_hit.collider == GetComponentInChildren<CapsuleCollider>())
            {
                ManageFriendlyLevel();
                friendshipLevelPanel.SetActive(true);
                //statePanel.SetActive(false);
            }
            else
            {
                friendshipLevelPanel.SetActive(false);
                //statePanel.SetActive(true);
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

        if (!m_isTalking)
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

    public void ShowLove()
    {
        //statePanel.SetActive(true);
        m_stateAnimator.SetTrigger("isCrying");
        m_gm.m_speechBubble.Talk(m_speech["greets"][0]);
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