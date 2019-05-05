using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public float meleeRate = 2f;
    public float specialRate = 5f;
    public float blockDisabledTime = 5f;
    public float movementSpeed = 20f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public string jumpAxis = "Jump_P1";
    public string horizontalAxis = "Horizontal_P1";
    public string fire1Axis = "Fire1_P1";
    public string fire2Axis = "Fire2_P1";
    public string specialAxis = "Special_P1";
    public string downPlatform = "DownPlatform_P1";

    internal bool m_isJumping;
    internal bool m_isBlocking;

    internal float m_meleeTimer;
    internal float m_specialTimer;
    internal float m_blockTimer;

    float m_horizontalMove = 0f;

    // This references shouldn't exist, changes should be made
    PlayerScript m_ps;
    CharacterController2D m_cc2d;

    void Awake()
    {
        m_ps = GetComponent<PlayerScript>();
        m_cc2d = GetComponent<CharacterController2D>();
    }

    void Start()
    {
        m_meleeTimer = meleeRate;
        m_blockTimer = blockDisabledTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ps.m_gm.m_timePaused)
        {
            return;
        }

        m_horizontalMove = Input.GetAxisRaw(horizontalAxis) * movementSpeed;

        if (Input.GetButtonDown(jumpAxis))
        {
            m_isJumping = true;
        }

        if(Input.GetButton(downPlatform))
        {
            // This doesn't take into account which player is hitting the platform, so all of them would fall down, not just one
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_cc2d.m_GroundCheck.position, CharacterController2D.k_GroundedRadius, m_cc2d.m_WhatIsGround);
            foreach(Collider2D cd in colliders)
            {
                if(cd.gameObject.GetComponentInParent<Platform2D>() != null)
                {
                    StartCoroutine(cd.gameObject.GetComponentInParent<Platform2D>().InvertRotationalOffset());
                }
            }

        }

        m_specialTimer += Time.deltaTime;
        if (Input.GetButton(specialAxis))
        {
            if (m_specialTimer >= specialRate)
            {
                StartCoroutine(SpecialAttack());
            }
        }

        m_meleeTimer += Time.deltaTime;
        if (Input.GetButton(fire1Axis))
        {
            if(m_meleeTimer >= meleeRate)
            {
                if(m_ps.m_controller.m_Grounded)
                {
                    m_ps.m_controller.m_canMove = false;
                    m_ps.m_animator.SetTrigger("melee" + Random.Range(1, 3));
                    m_meleeTimer = .0f;
                    m_ps.meleeCollider.gameObject.SetActive(true);
                }
            }
        }

        m_isBlocking = false;
        m_ps.m_controller.m_AirControl = true;
        m_blockTimer += Time.deltaTime;
        if (Input.GetButton(fire2Axis))
        {
            if (m_blockTimer >= blockDisabledTime)
            {
                m_isBlocking = true;
                if(m_ps.m_controller.m_Grounded)
                {
                    m_horizontalMove *= .3f;
                }else
                {
                    m_ps.m_controller.m_AirControl = false;
                }
            }
        }
        m_ps.m_animator.SetBool("blocking", m_isBlocking);
    }

    void FixedUpdate()
    {
        if (m_ps.m_gm.m_timePaused)
        {
            return;
        }

        m_ps.m_controller.Move(m_horizontalMove * Time.fixedDeltaTime, false, m_isJumping);

        m_isJumping = false;

        if (m_ps.m_rb.velocity.y < 0)
        {
            m_ps.m_rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (m_ps.m_rb.velocity.y > 0 && !Input.GetButton(jumpAxis))
        {
            m_ps.m_rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    IEnumerator SpecialAttack()
    {
        m_ps.m_animator.SetTrigger("special");
        m_ps.specialCollider.gameObject.SetActive(true);
        yield return new WaitForSeconds(.1f);
        m_ps.m_controller.Move((transform.position.x + 15) * ((m_ps.m_controller.isFacingRight) ? 1 : -1), false, false);
        m_ps.m_controller.m_canMove = false;
        m_specialTimer = 0;
    }
}
