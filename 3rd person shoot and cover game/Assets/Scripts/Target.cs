using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public float findNewPathTime;

    private bool m_inCoroutine = false;
    private bool m_isPathValid;

    private Vector2 m_mapBounds;

    private Vector3 m_target;

    private NavMeshAgent m_navMeshAgent;

    private NavMeshPath m_navPath;

    void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navPath = new NavMeshPath();

        m_mapBounds = new Vector2(40, 40);
    }

    void Update()
    {
        if(!m_inCoroutine)
        {
            StartCoroutine("DoSomething");
        }
    }

    public void TakeDamage()
    {
        Destroy(gameObject, 3f);
    }

    private Vector3 getNewRandomPosition()
    {
        float x = Random.Range(-m_mapBounds.x, m_mapBounds.x);
        float z = Random.Range(-m_mapBounds.y, m_mapBounds.y);

        return new Vector3(x, 1, z);
    }

    IEnumerator DoSomething()
    {
        m_inCoroutine = true;
        yield return new WaitForSeconds(findNewPathTime);

        while (!m_isPathValid)
        {
            yield return new WaitForSeconds(.01f);
            GetNewPath();
            m_isPathValid = m_navMeshAgent.CalculatePath(m_target, m_navPath);
        }

        m_inCoroutine = false;

    }

    private void GetNewPath()
    {
        m_target = getNewRandomPosition();
        m_navMeshAgent.SetDestination(m_target);
    }
}
