using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{

    public float movementSpeed = 5.0f;
    public float health = 100f;

    [Header("UI")]
    public Slider healthBar;
    public Image healthbarFill;

    private int current = 0;

    private float m_maxHealth = 100f;

    private Rigidbody m_rigidbody;

    private Vector3 m_prevTile;
    private Vector3 m_nextTile;

    private static GameManager m_gm;

    public static Vector3[] path = { new Vector3(-8.5f, -2.5f, 0), new Vector3(-6.5f, -2.5f, 0), new Vector3(-6.5f, 2.5f, 0), new Vector3(-1.5f, 2.5f, 0),
                                        new Vector3(-1.5f, -.5f, 0), new Vector3(-4.5f, -0.5f, 0), new Vector3(-4.5f, -2.5f, 0), new Vector3(-1.5f, -2.5f, 0),
                                        new Vector3(-1.5f, -4.5f, 0), new Vector3(-7.5f, -4.5f, 0), new Vector3(-7.5f, -6.5f, 0), new Vector3(.5f, -6.5f, 0),
                                        new Vector3(.5f, 2.5f, 0), new Vector3(2.5f, 2.5f, 0), new Vector3(2.5f, -.5f, 0), new Vector3(4.5f, -.5f, 0),
                                        new Vector3(4.5f, 2.5f, 0), new Vector3(6.5f, 2.5f, 0), new Vector3(6.5f, -2.5f, 0), new Vector3(2.5f, -2.5f, 0),
                                        new Vector3(2.5f, -5.5f, 0), new Vector3(11.5f, -5.5f, 0)};

    // Start is called before the first frame update
    void Start()
    {
        if(m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        m_rigidbody = GetComponent<Rigidbody>();

        health = m_maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if (m_gm.isMenuUp)
        {
            return;
        }

        if (current < path.Length)
        {
            if(transform.position != path[current])
            {
                Vector3 pos = Vector3.MoveTowards(transform.position, path[current], movementSpeed * Time.deltaTime);
                m_rigidbody.MovePosition(pos);
            }
            else
            {
                ++current;
            }
        }

        if(health >= m_maxHealth)
        {
            healthBar.transform.gameObject.SetActive(false);
        }
        else
        {
            healthBar.transform.gameObject.SetActive(true);

            healthBar.value = health;

            healthbarFill.color = Color.Lerp(Color.red, Color.green, (health / m_maxHealth));
        }

        if(health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float t_damage)
    {
        health -= t_damage;
    }

    void Die()
    {
        Destroy(transform.gameObject);
        --m_gm.aliveEnemies;
        m_gm.enemies.Remove(transform.gameObject);
        m_gm.gold += 5;
    }
}
