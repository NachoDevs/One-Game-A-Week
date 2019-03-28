using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    float m_movementSmoothing;
    float m_speed;

    Vector3 m_velocity;

    Rigidbody2D m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GetComponent<Enemy>().isControlled)
        {
            Fly();
        }
    }

    void Fly()
    {
        m_speed = Mathf.Sin(Time.time * 5) * 2;
        Vector3 targetVelocity = new Vector2(m_rigidbody.velocity.x, m_speed);
        // And then smoothing it out and applying it to the character
        m_rigidbody.velocity = Vector3.SmoothDamp(m_rigidbody.velocity, targetVelocity, ref m_velocity, m_movementSmoothing);
    }
}
