using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public Transform rallyPosition;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<NPC>() != null)
        {
            NPC npc = other.GetComponentInParent<NPC>();
            if (npc.home == gameObject)
            {
                npc.isHome = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<NPC>() != null)
        {
            NPC npc = other.GetComponentInParent<NPC>();
            if (npc.home == gameObject)
            {
                npc.isHome = false;
            }
        }
    }
}
