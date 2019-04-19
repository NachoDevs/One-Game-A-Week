using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public static int s_nextCharacterIndex;

    public static Dictionary<int, GameObject> id_prefab;

    public bool hasAttacked;
    public bool isDead;

    public int health = 100;
    public int damageBoost = 1;
    public int characterIndex;

    public string characterName;

    public Sprite deadSprite;

    public List<Ability> abilities;

    [HideInInspector]
    public Slider healthBar;

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

        if(id_prefab == null)
        {
            id_prefab = new Dictionary<int, GameObject>();
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        characterIndex = s_nextCharacterIndex++;

        if(!id_prefab.ContainsKey(characterIndex))
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

    public void UseAbility(Ability t_ability, Character t_target)
    {
        hasAttacked = true;
        switch (t_ability.type)
        {
            default:    // Melee ability is default
            case AbilityType.melee:
                m_animator.SetTrigger("melee");
                m_attacking = true;
                m_initialOrderInLayer = GetComponentInChildren<SpriteRenderer>().sortingOrder;
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 10;
                m_combatInitialPosition = transform.position;
                m_combatTarget = t_target;
                m_combatTarget.health -= t_ability.damage;
                break;
            case AbilityType.range:
                m_animator.SetTrigger("shoot");
                break;
            case AbilityType.special:
                print("special");
                break;
        }
    }

    void OnDestroy()
    {
        --s_nextCharacterIndex;
    }

    void Die()
    {
        isDead = true;
        m_animator.SetTrigger("die");
        m_animator.enabled = false;
        GetComponentInChildren<SpriteRenderer>().sprite = deadSprite;
    }
}
