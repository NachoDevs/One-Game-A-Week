using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2DMovement : MonoBehaviour
{

    public float movementSpeed = 20f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    bool m_isJumping;

    float m_horizontalMove = 0f;

    CharacterController2D m_controller;
    Rigidbody2D m_rb;

    void Awake()
    {
        m_controller = GetComponent<CharacterController2D>();
        m_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontalMove = Input.GetAxisRaw("Horizontal") * movementSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            m_isJumping = true;
        }

    }

    void FixedUpdate()
    {
        m_controller.Move(m_horizontalMove * Time.fixedDeltaTime, false, m_isJumping);

        m_isJumping = false;

        if (m_rb.velocity.y < 0)
        {
            m_rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (m_rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            m_rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
