using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitTypeBase : MonoBehaviour
{
    public AIIntentions intentions;

    protected GameObject target;

    protected NavMeshAgent m_navAgent;

    protected Unit m_unitRef;

    // Start is called before the first frame update
    protected void Start()
    {
        m_navAgent = GetComponentInChildren<NavMeshAgent>();
        m_unitRef = GetComponent <Unit> ();
    }

    // Update is called once per frame
    protected void Update()
    {
        HandleAI();
    }

    protected virtual void HandleAI()
    {
    }

    protected virtual void HandleInteractions(Collider t_other)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        HandleInteractions(other);
    }
}
