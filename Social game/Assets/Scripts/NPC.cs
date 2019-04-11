using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using SimpleJSON;
using System;

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
    Animator stateAnimator;

    Dictionary<string, List<string>> speech;

    List<GameObject> hearts;

    Vector3 m_targetPosition;
    Vector3 m_walkDirection;

    NavMeshAgent agent;

    RaycastHit hit;


    //////////////////////////

    List<Transform> interestPoints;
    List<GameObject> preferedItems;

    /////////////////////////
        
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        hearts = new List<GameObject>();

        LoadJSON();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("NPCMovement", .0f, 5);
        //friendshipLevel = 3;
        LoadJSON();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider == GetComponentInChildren<CapsuleCollider>())
            {
                ManageFriendlyLevel();
                friendshipLevelPanel.SetActive(true);
                statePanel.SetActive(false);
            }
            else
            {
                friendshipLevelPanel.SetActive(false);
                statePanel.SetActive(true);
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
            agent.SetDestination(m_targetPosition);
        }
    }

    void ManageFriendlyLevel()
    {
        if (hearts.Count > 0)
        {
            foreach(GameObject heart in hearts)
            {
                Destroy(heart.gameObject);
            }
        }

        for (int i = 0; i < (friendshipLevel / 2); ++i)
        {
            hearts.Add(Instantiate(fullHeartUI, friendshipLevelPanel.transform));
        }

        if(friendshipLevel % 2 != 0)
        {
            hearts.Add(Instantiate(halfHeartUI, friendshipLevelPanel.transform));
        }
    }

    public void ShowLove()
    {
        stateAnimator.SetTrigger("isCrying");
        print(speech["thanks"][0]);
    }

    private void LoadJSON()
    {

        string path = Application.dataPath + "/JSONs/speech.json";
        string jsonString = File.ReadAllText(path);

        JSONObject speechJSON = JSON.Parse(jsonString) as JSONObject;

        speech = new Dictionary<string, List<string>>();

        foreach(var personalityStuation in speechJSON)
        {
            if(personalityStuation.Key != personality.ToString())
            {
                continue;
            }

            foreach (var situationText in personalityStuation.Value)
            {
                speech.Add(situationText.Key, new List<string>());
                foreach (var text in situationText.Value)
                {
                    speech[situationText.Key].Add(text.Value);
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