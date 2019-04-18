using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    melee,
    range,
    special
}

public class Ability
{
    public int damage;

    public AbilityType type;

    public Ability()
    {
    }

    public void SetAbilityAs(string t_type)
    {
        Enum.TryParse(t_type, out type);

        switch(type)
        {
            default:    // Melee ability is default
            case AbilityType.melee:
                damage = 25;
                type = AbilityType.melee;
                break;
            case AbilityType.range:
                damage = 30;
                type = AbilityType.range;
                break;
            case AbilityType.special:
                damage = 60;
                type = AbilityType.special;
                break;
        }
    }
}
