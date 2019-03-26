using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameManager m_gm;

    void Awake()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if(m_gm.m_timePaused)
        {
            return;
        }
    }

}
