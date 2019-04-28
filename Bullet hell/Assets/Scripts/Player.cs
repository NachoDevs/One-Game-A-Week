using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float fireRate = 2.0f;

    public GameObject projectile;

    bool isShielded;

    float m_timer;

    SpriteRenderer m_sprite;

    // Start is called before the first frame update
    void Start()
    {
        isShielded = true;
        m_sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if(isShielded)
        {
            m_sprite.color = Color.Lerp(Color.white, Color.cyan, Mathf.PingPong(Time.time, 1));
        }

        if (Input.GetAxis("Fire1") > 0)
        {
            if (m_timer >= fireRate)
            {
                m_timer = .0f;
                Shoot();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Shield>() != null)
        {
            if(isShielded)
            {
                return;
            }
            isShielded = true;
        }

        Destroy(collision.gameObject);

        if(!isShielded)
        {
            Die();
        }
        else
        {
            isShielded = false;
        }
    }

    void Shoot()
    {
        Vector3 instPos = transform.position + (transform.right * .6f);
        GameObject proj = Instantiate(projectile, instPos, transform.rotation);
        proj.GetComponent<Projectile>().isPlayers = true;
        proj.GetComponent<Rigidbody2D>().AddForce(transform.right * 1750);
    }

    void Die()
    {
        print("dead");

        Destroy(transform.gameObject);

        SceneManager.LoadScene(0);
    }
}
