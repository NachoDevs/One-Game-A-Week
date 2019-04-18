using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    melee,
    range,
    special
}

public class Ability
{
    public int damage;

    public AttackType type;

    public Ability()
    {
    }

    public void SetAbilityAs(string t_type)
    {
        Enum.TryParse(t_type, out type);

        switch(type)
        {
            default:    // Melee ability is default
            case AttackType.melee:
                damage = 3;
                type = AttackType.melee;
                break;
            case AttackType.range:
                damage = 5;
                type = AttackType.range;
                break;
            case AttackType.special:
                damage = 10;
                type = AttackType.special;
                break;
        }
    }
}
