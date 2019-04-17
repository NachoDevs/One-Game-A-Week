using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    melee,
    range,
    pacifist
}

public class Character : MonoBehaviour
{
    public int health;
    public int attackDamage;

    public AttackType attackType;

    CharacterMovement m_cm;

    void Awake()
    {
        if(GetComponent<CharacterMovement>() != null)
        {
            m_cm = GetComponent<CharacterMovement>();
        }
    }

    public void MoveTo(WorldTile t_destination)
    {
        if(m_cm != null)
        {
            m_cm.MoveTo(t_destination);
        }
    }
}
