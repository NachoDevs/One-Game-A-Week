using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    SpriteRenderer m_sprite;

    void Start()
    {
        m_sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        m_sprite.color = Color.Lerp(Color.white, Color.cyan, Mathf.PingPong(Time.time, 1));
    }
}
