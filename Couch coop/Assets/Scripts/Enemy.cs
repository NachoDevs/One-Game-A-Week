using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float movementSpeed = 5;

    public Transform groundCheck;
    public Transform wallCheck;

    bool m_isControlled  = false;
    bool m_isFacingRight = false;

    float m_movementSmoothing;
    float t_speed;

    Vector3 m_velocity;

    CapsuleCollider2D m_deathCollider;

    Rigidbody2D m_rigidbody;

    [SerializeField] LayerMask m_WhatIsEnvironment;

    const float CHECKRADIUS = .2f;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_deathCollider = GetComponentInChildren<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_isControlled)
        {
            Move();
        }
    }

    void FixedUpdate()
    {
        // Checking if there is ground infront
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, CHECKRADIUS, m_WhatIsEnvironment);
        if (colliders.Length == 0)
        {
            Flip();
        }

        // Checking if there is a wall infront
        colliders = Physics2D.OverlapCircleAll(groundCheck.position, CHECKRADIUS, m_WhatIsEnvironment);
        if (colliders.Length == 0)
        {
            Flip();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<Player>() != null)
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        t_speed = movementSpeed * Time.deltaTime * transform.localScale.x;
        Vector3 targetVelocity = new Vector2(t_speed, m_rigidbody.velocity.y);
        // And then smoothing it out and applying it to the character
        m_rigidbody.velocity = Vector3.SmoothDamp(m_rigidbody.velocity, targetVelocity, ref m_velocity, m_movementSmoothing);

        // If the input is moving the player right and the player is facing left...
        /*if (t_speed > 0 && !m_isFacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (t_speed < 0 && m_isFacingRight)
        {
            // ... flip the player.
            Flip();
        }*/
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_isFacingRight = !m_isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
