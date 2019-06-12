using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState
{
    Idle,
    ReturnToPatrol,
    Patrol,
    Pursuit,
    Searching,

}

public class Enemy : MonoBehaviour
{
    public GameObject questionMark;
    public GameObject exclamationMark;

    public List<Transform> patrolCheckpoints;

    [Header("Materials")]
    public Material mat_Idle;
    public Material mat_Patrol;
    public Material mat_Pursuit;
    public Material mat_Searching;

    static GameManager m_gm;

    bool m_TimerStarted;
    bool m_mooving;
    bool m_canSeePlayer;

    int m_checkpointIndex;

    float m_waitCounter;
    float m_waitTime;

    Vector3 m_targetPos;
    Vector3 m_idleCenter;

    EnemyState m_currentState;

    GameObject m_currentMark;

    NavMeshAgent m_navAgent;

    Renderer m_renderer;

    void Awake()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_renderer = GetComponentInChildren<Renderer>();

        m_waitTime = 5;

        ChangeBehaviour(EnemyState.ReturnToPatrol);

        m_TimerStarted = false;

        m_idleCenter = Vector3.zero;
    }

    void Start()
    {
        if(m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    }

    void Update()
    {
        CheckPlayer();
        HandleBehaviour();
    }

    void CheckPlayer()
    {
        float result = Vector3.Dot(Vector3.Normalize(transform.position - m_gm.player.transform.position), transform.forward);
        if (result < -.45f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, m_gm.player.transform.position - transform.position, out hit, 25f))
            {
                Debug.DrawRay(transform.position, m_gm.player.transform.position - transform.position, Color.red);
                m_canSeePlayer = hit.collider.GetComponentInParent<Player>() != null;
                if (m_canSeePlayer)
                {
                    m_targetPos = hit.point;
                    ResetTimer();
                    ChangeBehaviour(EnemyState.Pursuit);
                }
            }
        }
    }

    void HandleBehaviour()
    {
        switch (m_currentState)
        {
            default:
            case EnemyState.Idle:

                if (m_TimerStarted)
                {
                    m_waitCounter += Time.deltaTime;

                    if (!(m_waitTime > m_waitCounter))
                    {
                        print("end wait");
                        StopTimer();
                    }
                    break;
                }

                if (IsCloseToPosition(m_targetPos))
                {
                    StartTimer(5);
                    m_mooving = false;
                    m_targetPos = m_idleCenter + FindNewRandomPositionInRadius(1);

                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Vector3 newPos = m_targetPos;
                    newPos.y += 3;
                    go.transform.position = newPos;
                    Destroy(go, 2);

                    return;
                }

                if(!m_mooving)
                {
                    MoveTo(m_targetPos);
                }
                break;
            case EnemyState.ReturnToPatrol:
                if (IsCloseToPosition(m_targetPos))
                {
                    ChangeBehaviour(EnemyState.Patrol);
                }

                if (!m_mooving)
                {
                    m_targetPos = FindClosestCheckpoint();
                    MoveTo(m_targetPos);
                }
                break;
            case EnemyState.Patrol:
                if(IsCloseToPosition(m_targetPos))
                {
                    SetNextPatrolPosition();
                    MoveTo(m_targetPos + FindNewRandomPositionInRadius(2));
                }
                break;
            case EnemyState.Pursuit:
                MoveTo(m_targetPos);

                if(IsCloseToPosition(m_targetPos))
                {
                    ChangeBehaviour(EnemyState.Searching);
                }

                break;
            case EnemyState.Searching:
                transform.Rotate(Vector3.up * 150 * Time.deltaTime, Space.Self);

                if (m_TimerStarted)
                {
                    m_waitCounter += Time.deltaTime;

                    if (m_waitTime <= m_waitCounter)
                    {
                        ChangeBehaviour(EnemyState.ReturnToPatrol);
                        StopTimer();
                    }
                    break;
                }

                StartTimer(5);
                break;
        }
    }

    void ChangeBehaviour(EnemyState t_state)
    {
        m_currentState = t_state;
        switch (t_state)
        {
            default:
            case EnemyState.Idle:
                m_renderer.material = mat_Idle;
                break;
            case EnemyState.ReturnToPatrol:
            case EnemyState.Patrol:
                m_renderer.material = mat_Patrol;
                break;
            case EnemyState.Pursuit:
                StartCoroutine(ShowMark(exclamationMark));
                m_renderer.material = mat_Pursuit;
                break;
            case EnemyState.Searching:
                StartCoroutine(ShowMark(questionMark));
                m_renderer.material = mat_Searching;
                break;
        }
    }

    void ResetTimer()
    {
        m_waitCounter = 0;
        m_TimerStarted = false;
    }

    void StartTimer(float t_time)
    {
        if(!m_TimerStarted)
        {
            m_waitTime = t_time;
            m_waitCounter = 0;
            m_TimerStarted = true;
        }
    }

    void StopTimer()
    {
        m_waitTime = 0;
        m_TimerStarted = false;
    }

    void MoveTo(Vector3 t_newPos)
    {
        m_mooving = true;
        m_targetPos = t_newPos;
        m_navAgent.SetDestination(m_targetPos);
    }

    void SetNextPatrolPosition()
    {
        m_targetPos = patrolCheckpoints[m_checkpointIndex++].position;
        m_checkpointIndex %= patrolCheckpoints.Count;
    }

    bool IsCloseToPosition(Vector3 t_target)
    {
        return Mathf.Abs(t_target.x - transform.position.x) < .1f && Mathf.Abs(t_target.z - transform.position.z) < .1f;
    }

    Vector3 FindClosestCheckpoint()
    {
        float maxDistance = float.MaxValue;
        Vector3 targetPos = m_targetPos;

        foreach(Transform trans in patrolCheckpoints)
        {
            float dist = Vector3.Distance(trans.position, transform.position);
            if(dist < maxDistance)
            {
                targetPos = trans.position;
                maxDistance = dist;
            }
        }

        return targetPos;
    }

    Vector3 FindNewRandomPositionInRadius(int t_radius)
    {
        Vector2 randomPos = UnityEngine.Random.insideUnitCircle * t_radius;
        return new Vector3(randomPos.x, transform.position.y, randomPos.y);
    }

    IEnumerator ShowMark(GameObject t_mark)
    {
        Vector3 markPos = transform.position;
        markPos.y += 1;
        Destroy(m_currentMark);
        m_currentMark = Instantiate(t_mark, markPos, Quaternion.identity, transform);
        yield return new WaitForSeconds(2f);
        Destroy(m_currentMark);
    }
}
