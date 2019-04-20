using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public static int s_nextCharacterIndex;

    public static Dictionary<int, GameObject> id_prefab;

    public bool hasMoved;
    public bool hasAttacked;
    public bool isDead;

    public int health = 100;
    public int damageBoost = 1;
    //[HideInInspector]
    public int characterIndex = -1;

    public string characterName;

    public Sprite deadSprite;

    public List<Ability> abilities;

    public Vector3 combatInitialPosition;

    [HideInInspector]
    public Slider healthBar;

    bool m_isAttacking = false;

    int m_initialOrderInLayer;
    int m_maxHealth = 100;

    float m_combatSpeed = 6;

    CharacterMovement m_cm;

    Animator m_animator;

    Character m_combatTarget;

    void Awake()
    {
        characterIndex = -1;

        abilities = new List<Ability>();
        m_animator = GetComponentInChildren<Animator>();

        if (GetComponent<CharacterMovement>() != null)
        {
            m_cm = GetComponent<CharacterMovement>();
        }

        if(id_prefab == null)
        {
            id_prefab = new Dictionary<int, GameObject>();
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        CheckIndex();

        if (!id_prefab.ContainsKey(characterIndex))
        {
            id_prefab.Add(characterIndex, gameObject);
        }
    }

    void Update()
    {
        if(healthBar != null)
        {
            healthBar.value = health;
        }

        if(!isDead)
        {
            if(health < 1)
            {
                Die();
            }
        }

        if(m_combatTarget != null)
        {
            if (m_isAttacking)
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
                    m_isAttacking = false;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, combatInitialPosition) > .1f)
                {
                    Vector3 directionOfTravel = combatInitialPosition - transform.position;
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

    public void CheckIndex()
    {
        if (characterIndex == -1)
        {
            characterIndex = s_nextCharacterIndex++;
        }
    }

    public void MoveTo(WorldTile t_destination)
    {
        if(m_cm != null)
        {
            m_cm.MoveTo(t_destination);
        }
    }

    public void UseAbility(Ability t_ability, Character t_target)
    {
        m_isAttacking = true;
        hasAttacked = true;

        if(t_ability.damage > 0)
        {
            t_target.health -= t_ability.damage;
        }

        switch (t_ability.type)
        {
            default:    // Melee ability is default
            case AbilityType.melee:
                m_animator.SetTrigger("melee");
                m_initialOrderInLayer = GetComponentInChildren<SpriteRenderer>().sortingOrder;
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 10;
                //combatInitialPosition = transform.position;
                m_combatTarget = t_target;
                break;
            case AbilityType.range:
                m_animator.SetTrigger("shoot");
                break;
            case AbilityType.heal:
                health += 15;
                if(health > m_maxHealth)
                {
                    health = m_maxHealth;
                }
                break;
        }
    }

    public void NewTurn()
    {
        hasMoved = false;
    }

    public void NewCombatTurn()
    {
        hasAttacked = false;
        m_isAttacking = false;
        m_combatTarget = null;
    }

    void OnDestroy()
    {
        --s_nextCharacterIndex;
    }

    public void Die()
    {
        isDead = true;
        m_animator.SetTrigger("die");
        m_animator.enabled = false;
        GetComponentInChildren<SpriteRenderer>().sprite = deadSprite;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
    }
}
