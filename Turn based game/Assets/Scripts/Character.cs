using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    static int nextCharacterIndex;

    public bool hasAttacked;

    public int health = 100;
    public int attackDamageBoost = 1;
    public int characterIndex;

    public string name;

    public List<Ability> abilities;

    bool m_attacking = false;

    int m_initialOrderInLayer;

    float m_combatSpeed = 6;

    CharacterMovement m_cm;

    Animator m_animator;

    Vector3 m_combatInitialPosition;

    Character m_combatTarget;

    void Awake()
    {
        abilities = new List<Ability>();
        m_animator = GetComponentInChildren<Animator>();

        if (GetComponent<CharacterMovement>() != null)
        {
            m_cm = GetComponent<CharacterMovement>();
        }
        characterIndex = nextCharacterIndex++;
    }

    void Start()
    {
    }

    void Update()
    {
        if(m_combatTarget != null)
        {
            if (m_attacking)
            {
                if (Vector3.Distance(transform.position, m_combatTarget.gameObject.transform.position) > .1f)
                {
                    Vector3 directionOfTravel = m_combatTarget.gameObject.transform.position - transform.position;
                    directionOfTravel.Normalize();

                    transform.Translate(
                        (directionOfTravel.x * m_combatSpeed * Time.deltaTime),
                        (directionOfTravel.y * m_combatSpeed * Time.deltaTime),
                        (directionOfTravel.z * m_combatSpeed * Time.deltaTime),
                        Space.World);
                }
                else
                {
                    m_attacking = false;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, m_combatInitialPosition) > .1f)
                {
                    Vector3 directionOfTravel = m_combatInitialPosition - transform.position;
                    directionOfTravel.Normalize();

                    transform.Translate(
                        (directionOfTravel.x * m_combatSpeed * Time.deltaTime),
                        (directionOfTravel.y * m_combatSpeed * Time.deltaTime),
                        (directionOfTravel.z * m_combatSpeed * Time.deltaTime),
                        Space.World);
                }
                else
                {
                    m_combatTarget = null;
                    GetComponentInChildren<SpriteRenderer>().sortingOrder = m_initialOrderInLayer;
                }
            }
        }
    }

    public void MoveTo(WorldTile t_destination)
    {
        if(m_cm != null)
        {
            m_cm.MoveTo(t_destination);
        }
    }

    public void UseAbility(AbilityType t_ability, Character t_target)
    {
        switch (t_ability)
        {
            default:    // Melee ability is default
            case AbilityType.melee:
                m_animator.SetTrigger("melee");
                m_attacking = true;
                m_initialOrderInLayer = GetComponentInChildren<SpriteRenderer>().sortingOrder;
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 10;
                m_combatInitialPosition = transform.position;
                m_combatTarget = t_target;
                break;
            case AbilityType.range:
                m_animator.SetTrigger("shoot");
                break;
            case AbilityType.special:
                print("special");
                break;
        }
    }
}
