using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    Vector3 respawnPos = new Vector3(.0f, 2.5f, .0f);

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<PlayerScript>() != null)
        {
            collision.transform.parent.transform.position = respawnPos;
        }
        else
        {
            Destroy(collision.transform.parent.transform);
        }
    }
}
