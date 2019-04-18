using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool hasAttacked;

    public int health;
    public int attackDamageBoost = 1;
    public int characterIndex;

    public string name;


    public List<Ability> abilities;

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

    void Start()
    {
    }

    public void MoveTo(WorldTile t_destination)
    {
        if(m_cm != null)
        {
            m_cm.MoveTo(t_destination);
        }
    }
}
