using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject blockEffect;

    public BoxCollider2D meleeCollider;
    public PolygonCollider2D specialCollider;

    internal Animator m_animator;

    internal CharacterController2D m_controller;

    internal PlayerInputManager m_pInput;

    internal Rigidbody2D m_rb;

    internal GameManager m_gm;

    void Awake()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        m_controller = GetComponent<CharacterController2D>();
        m_rb = GetComponent<Rigidbody2D>();
        m_pInput = GetComponent<PlayerInputManager>();
        m_animator = GetComponentInChildren<Animator>();
    }

    public void MeleeDamaged()
    {
        if (m_pInput.m_isBlocking)
        {
            m_pInput.m_isBlocking = false;
            m_pInput.m_blockTimer = 0;
            Vector3 blockPos = transform.position;
            blockPos.y -= 0.2f;
            Destroy(Instantiate(blockEffect, blockPos, Quaternion.identity), .35f);
        }
        else
        {
            Die();
        }
    }

    public void SpecialDamaged(GameObject t_target)
    {
        if (m_pInput.m_isBlocking)
        {
            t_target.GetComponent<Rigidbody2D>().AddForce(new Vector2(10f * ((m_controller.isFacingRight) ? 1 : -1), 200f));
        }

        MeleeDamaged();
    }

    void Die()
    {
        m_controller.m_canMove = false;
        m_animator.SetTrigger("dead");
    }
}
