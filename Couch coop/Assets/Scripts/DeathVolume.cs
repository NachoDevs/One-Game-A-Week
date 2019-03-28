using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVolume : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<Rigidbody2D>() != null)
        {
            if(collision.GetComponentInParent<Player>() != null)
            {
                collision.transform.parent.position = new Vector3();
            }
            else
            {
                Destroy(collision.transform.parent.gameObject);
            }
        }
    }
}
