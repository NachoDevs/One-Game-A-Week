using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    public bool isPlayer = false;

    public Camera cam;

    float m_walkRadius = 5;

    Vector3 m_walkDirection;

    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if(!isPlayer)
        {
            InvokeRepeating("NPCBehaviour", .0f, 5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(isPlayer)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }

    void NPCBehaviour()
    {
        m_walkDirection = Random.insideUnitSphere * m_walkRadius;
        m_walkDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(m_walkDirection, out hit, m_walkRadius, 1);

        agent.SetDestination(hit.position);

    }
}
