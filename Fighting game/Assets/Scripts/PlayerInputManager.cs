using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public float meleeRate = 2f;
    public float movementSpeed = 20f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    bool m_isJumping;
    bool m_isBlocking;

    float m_horizontalMove = 0f;
    float m_meleeTimer = 2.0f;

    CharacterController2D m_controller;
    Rigidbody2D m_rb;

    GameManager m_gm;

    void Awake()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        m_controller = GetComponent<CharacterController2D>();
        m_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_gm.m_timePaused)
        {
            return;
        }

        m_horizontalMove = Input.GetAxisRaw("Horizontal") * movementSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            m_isJumping = true;
        }

        m_meleeTimer += Time.deltaTime;
        if (Input.GetButton("Fire1"))
        {
            if(m_meleeTimer >= meleeRate)
            {
                if(m_controller.m_Grounded)
                {
                    m_controller.m_canMove = false;
                    m_controller.m_animator.SetTrigger("melee" + Random.Range(1, 3));
                    m_meleeTimer = .0f;
                }
            }
        }

        m_isBlocking = false;
        m_controller.m_AirControl = true;
        if (Input.GetButton("Fire2"))
        {
            m_isBlocking = true;
            if(m_controller.m_Grounded)
            {
                m_horizontalMove *= .3f;
            }else
            {
                m_controller.m_AirControl = false;
            }
        }
        m_controller.m_animator.SetBool("blocking", m_isBlocking);
    }

    void FixedUpdate()
    {
        if (m_gm.m_timePaused)
        {
            return;
        }

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
