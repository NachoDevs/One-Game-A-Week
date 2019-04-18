using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    melee,
    range,
    both,
    pacifist
}

public class Character : MonoBehaviour
{
    public int health;
    public int attackDamage;
    public int characterIndex;

    public string name;

    public AttackType attackType;

    static int nextCharacterIndex;

    CharacterMovement m_cm;

    void Awake()
    {
        if(GetComponent<CharacterMovement>() != null)
        {
            m_cm = GetComponent<CharacterMovement>();
        }
        characterIndex = ++nextCharacterIndex;
    }

    public void MoveTo(WorldTile t_destination)
    {
        if(m_cm != null)
        {
            m_cm.MoveTo(t_destination);
        }
    }
}
