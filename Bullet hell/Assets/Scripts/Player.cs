using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float fireRate = 2.0f;

    public GameObject projectile;

    float m_timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if (Input.GetAxis("Fire1") > 0)
        {
            if (m_timer >= fireRate)
            {
                m_timer = .0f;
                Shoot();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    void Shoot()
    {
        Vector3 instPos = transform.position + (transform.right * .6f);
        GameObject proj = Instantiate(projectile, instPos, transform.rotation);
        proj.GetComponent<Rigidbody2D>().AddForce(transform.right * 500);
    }

    void Die()
    {
        print("dead");

        //Destroy(transform.gameObject);
    }
}
