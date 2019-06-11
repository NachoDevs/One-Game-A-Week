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
    public List<Transform> patrolCheckpoints;

    bool m_waiting;
    bool m_mooving;

    int m_checkpointIndex;

    float m_waitCounter;
    float m_waitTime;

    Vector3 m_targetPos;
    Vector3 m_idleCenter;

    EnemyState m_currentState;

    NavMeshAgent m_navAgent;

    void Awake()
    {
        m_navAgent = GetComponent<NavMeshAgent>();

        m_waitTime = 5;
        m_currentState = EnemyState.ReturnToPatrol;

        m_waiting = false;

        m_idleCenter = Vector3.zero;
    }

    void Update()
    {
        HandleBehaviour();
    }

    void HandleBehaviour()
    {
        if(m_waiting)
        {
            m_waitCounter += Time.deltaTime;

            if(m_waitTime > m_waitCounter)
            {
                return;
            }
            StopTimer();
        }

        switch (m_currentState)
        {
            default:
            case EnemyState.Idle:

                if (IsCloseToPosition(m_targetPos))
                {
                    StartTimer(.1f);
                    m_mooving = false;
                    m_targetPos = m_idleCenter + FindNewRandomePositionInRadius(5);
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
                    m_currentState = EnemyState.Patrol;
                }
                else
                {
                    if (!m_mooving)
                    {
                        m_targetPos = FindClosestCheckpoint();
                        MoveTo(m_targetPos);
                    }
                }
                break;
            case EnemyState.Patrol:
                if(IsCloseToPosition(m_targetPos))
                {
                    SetNextPatrolPosition();
                    MoveTo(m_targetPos + FindNewRandomePositionInRadius(2));
                }
                break;
            case EnemyState.Pursuit:
                break;
            case EnemyState.Searching:
                break;
        }
    }

    void StartTimer(float t_time)
    {
        m_waitTime = t_time;
        m_waitCounter = 0;
        m_waiting = true;
    }

    void StopTimer()
    {
        m_waitTime = 0;
        m_waiting = false;
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

    Vector3 FindNewRandomePositionInRadius(int t_radius)
    {
        Vector2 randomPos = Random.insideUnitCircle * t_radius;
        return new Vector3(randomPos.x, transform.position.y, randomPos.y);
    }
}
