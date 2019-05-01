using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    CharacterController2D m_cc;

    void Start()
    {
        m_cc = GetComponentInParent<CharacterController2D>();
    }

    public void CanMoveAgain()
    {
        m_cc.m_canMove = true;
    }
}
