using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : UnitTypeBase
{
    [SerializeField]
    int maxCarryingAmount;

    [SerializeField]
    int currentCarryingAmount;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        intentions = AIIntentions.collectResources;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void HandleAI()
    {
        base.HandleAI();

        switch (intentions)
        {
            default:
            case AIIntentions.idle:
                StartCoroutine(CheckForNewResources());
                return;
            case AIIntentions.collectResources:
                // TODO: change if more resources are added
                SetTargetToClosestResource(ResourceType.greeResource);
                break;
            case AIIntentions.deliverResources:
                SetTargetToClosestCollector();
                break;
        }
        // No target found
        if (target == null)
        {
            if(currentCarryingAmount > 0)
            {
                intentions = AIIntentions.deliverResources;
            }
            else
            {
                intentions = AIIntentions.idle;
            }
            return;
        }
        m_navAgent.SetDestination(target.transform.position);
    }

    protected override void HandleInteractions(Collider t_other)
    {
        switch (intentions)
        {
            default:
            case AIIntentions.idle:
                return;
            case AIIntentions.collectResources:
                Resource resource = null;
                try
                {
                    resource = t_other.GetComponentInParent<Resource>();
                }
                catch (Exception e) { GameManager.PrintException(e); break; }

                if (resource != null)
                {
                    currentCarryingAmount += (maxCarryingAmount <= resource.resourcesLeft) ? maxCarryingAmount : resource.resourcesLeft;
                    resource.resourcesLeft -= currentCarryingAmount;

                    if (resource.resourcesLeft <= 0)
                    {
                        Destroy(resource.gameObject);
                    }

                    if (currentCarryingAmount >= maxCarryingAmount)
                    {
                        intentions = AIIntentions.deliverResources;
                    }
                }
                break;
            case AIIntentions.deliverResources:
                Collector building = null;
                try
                {
                    building = t_other.GetComponentInParent<Collector>();
                }
                catch (Exception e) { GameManager.PrintException(e); break; }

                if (building != null)
                {
                    currentCarryingAmount = 0;

                    intentions = AIIntentions.collectResources;
                }
                break;
        }
    }

    void SetTargetToClosestResource(ResourceType t_resType)
    {
        float minDistance = float.MaxValue;
        foreach (Transform res in Resource.resourcesParent)
        {
            try
            {
                if(res.GetComponent<Resource>() == null)
                {
                    continue;
                }
            }
            catch (Exception e) { GameManager.PrintException(e); continue; }

            float distance = Vector3.Distance(transform.position, res.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = res.gameObject;
            }
        }
    }

    void SetTargetToClosestCollector()
    {
        float distance = 0;
        float minDistance = float.MaxValue;
        Transform teamParent = GameObject.Find("teamParent_" + m_unitRef.teamName).transform;
        foreach (Transform building in teamParent)
        {
            // If the building doesn't is a collector, we continue iterating
            try
            {
                if(building.GetComponent<Collector>() == null)
                {
                    continue;
                }
            }
            catch (Exception e) { GameManager.PrintException(e); continue; }

            distance = Vector3.Distance(transform.position, building.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = building.gameObject;
            }
        }
    }

    IEnumerator CheckForNewResources()
    {
        yield return new WaitForSeconds(2);

        if (currentCarryingAmount < maxCarryingAmount)
        {
            intentions = AIIntentions.collectResources;
        }
        else
        {
            intentions = AIIntentions.deliverResources;
        }
    }

}
