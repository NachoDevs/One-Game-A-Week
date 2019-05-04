using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    PlayerScript m_ps;

    void Awake()
    {
        m_ps = GetComponentInParent<PlayerScript>();
    }

    public void CanMoveAgain()
    {
        m_ps.m_controller.m_canMove = true;
        m_ps.meleeCollider.gameObject.SetActive(false);
        m_ps.specialCollider.gameObject.SetActive(false);
    }
}
