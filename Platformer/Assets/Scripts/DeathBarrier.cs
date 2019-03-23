using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<Player>() != null)
        {
            collision.transform.parent.position = new Vector3(0, 0, 0);
        }
        else
        {
            Destroy(collision.transform.parent.gameObject);
        }
    }
}
