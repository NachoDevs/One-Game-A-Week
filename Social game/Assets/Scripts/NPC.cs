using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    public bool m_isTalking = false;

    public int friendshipLevel;

    public GameObject friendshipLevelPanel;
    public GameObject fullHeartUI;
    public GameObject halfHeartUI;

    readonly int maxFriendshipLevel = 6;

    readonly float m_walkRadius = 5;

    List<GameObject> hearts;

    Vector3 m_targetPosition;
    Vector3 m_walkDirection;

    NavMeshAgent agent;

    RaycastHit hit;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("NPCMovement", .0f, 5);
        //friendshipLevel = 3;
        hearts = new List<GameObject>();
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
            }
            else
            {
                friendshipLevelPanel.SetActive(false);
            }
        }
    }

    void NPCMovement()
    {
        m_walkDirection = Random.insideUnitSphere * m_walkRadius;
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
}
