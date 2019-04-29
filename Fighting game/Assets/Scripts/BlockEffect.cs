using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEffect : MonoBehaviour
{
    Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponentInChildren<Animator>();

        m_animator.SetTrigger("block3");
        //m_animator.SetTrigger("block" + Random.Range(0, 3));
    }
}
