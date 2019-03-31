using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isControlled  = false;

    public float movementSpeed = 5;

    public GameObject selectedImage;

    bool m_isFacingRight = true;

    float m_movementSmoothing;
    float m_speed;
    float m_horizontalMove;

    Vector3 m_velocity;
    Vector3 m_targetVelocity;

    CapsuleCollider2D m_deathCollider;

    Rigidbody2D m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_deathCollider = GetComponentInChildren<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isControlled)
        {
            m_horizontalMove = Input.GetAxisRaw("HorizontalMasterMind") * movementSpeed;
        }
        Move();
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
        if (isControlled)
        {
            m_speed = m_horizontalMove * Time.fixedDeltaTime;
            m_targetVelocity = new Vector2(m_speed, m_rigidbody.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (m_speed > 0 && !m_isFacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (m_speed < 0 && m_isFacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        else
        {
            m_speed = movementSpeed * Time.deltaTime * transform.localScale.x;
            m_targetVelocity = new Vector2(m_speed, m_rigidbody.velocity.y);
            // And then smoothing it out and applying it to the character
        }
        m_rigidbody.velocity = Vector3.SmoothDamp(m_rigidbody.velocity, m_targetVelocity, ref m_velocity, m_movementSmoothing);
    }

    public void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_isFacingRight = !m_isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
