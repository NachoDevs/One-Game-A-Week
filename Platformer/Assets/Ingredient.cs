using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{

    BoxCollider2D m_collider;
    CircleCollider2D m_detectionArea;

    void Awake()
    {
        m_collider = GetComponentInChildren<BoxCollider2D>();
        m_detectionArea = GetComponentInChildren<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //if(m_detectionArea.IsTouching(collision))
        //{
            collision.transform.position += Vector3.MoveTowards(transform.position, collision.transform.position, Time.deltaTime);
        //}
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}
