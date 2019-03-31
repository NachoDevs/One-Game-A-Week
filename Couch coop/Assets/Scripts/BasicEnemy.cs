using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    const float k_checkRadius = .2f;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;

    [SerializeField] LayerMask m_whatIsEnvironment;

    Enemy m_enemyRef;

    void Awake()
    {
        m_enemyRef = GetComponent<Enemy>();
    }

    void FixedUpdate()
    {
        if (!m_enemyRef.isControlled)
        {
            // Checking if there is ground in front
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, k_checkRadius, m_whatIsEnvironment);
            if (colliders.Length == 0)
            {
                m_enemyRef.Flip();
            }

            // Checking if there is a wall in front
            colliders = Physics2D.OverlapCircleAll(wallCheck.position, k_checkRadius, m_whatIsEnvironment);
            if (colliders.Length > 0)
            {
                m_enemyRef.Flip();
            }
        }
    }
}
