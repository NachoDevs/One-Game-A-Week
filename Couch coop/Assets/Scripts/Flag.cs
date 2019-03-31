using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] BoxCollider2D m_poleCollider;
    [SerializeField] CircleCollider2D m_topCollider;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>() != null)
        {
            //if (collision == m_poleCollider)
            //{
            //    print("top!!");
            //}

            Destroy(collision.transform.parent.gameObject);
        }
    }

}
