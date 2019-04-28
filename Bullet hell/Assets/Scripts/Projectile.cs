using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    static GameController m_gc;

    public bool isPlayers;

    float m_timer;

    void Start()
    {
        if(m_gc == null)
        {
            m_gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
    }

    void Update()
    {
        m_timer += Time.deltaTime;

        if(m_timer >= 15)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(isPlayers)
        {
            m_gc.IncreaseScore((collision.gameObject.GetComponentInParent<Enemy>() != null) ? 25 : 10);
        }
        Destroy(gameObject);
    }
}
