using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    PlayerScript m_target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerScript>() != null)
        {
            m_target = collision.GetComponentInParent<PlayerScript>();
            if (GetComponent<PolygonCollider2D>() != null)   // Special attack is been used
            {
                m_target.SpecialDamaged(m_target.gameObject);
            }
            else
            {
                m_target.MeleeDamaged();
            }
        }
    }
}
