using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public Sprite ingredientSprite;

    bool m_haveTarget = false;

    Vector3 m_target;

    BoxCollider2D m_collider;
    CircleCollider2D m_detectionArea;

    static GameManager m_gm;

    void Awake()
    {
        if(m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        m_collider = GetComponentInChildren<BoxCollider2D>();
        m_detectionArea = GetComponentInChildren<CircleCollider2D>();

        GetComponentInChildren<SpriteRenderer>().sprite = ingredientSprite;
    }

    void Update()
    {
        if (m_gm.m_timePaused)
        {
            return;
        }

        if (m_haveTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_target, Time.deltaTime * 10);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponentInParent<Player>() != null)
        {
            m_haveTarget = true;
            m_target = collision.transform.position;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Player p = collision.gameObject.GetComponentInParent<Player>();
        if (p != null)
        {
            p.TakeIngredient(this);
            m_haveTarget = false;
        }
    }

    public IEnumerator EnableIngredientColliders(bool t_enable)
    {
        m_haveTarget = false;

        if (t_enable)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }
        else
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
        }

        yield return new WaitForSeconds((t_enable) ? .5f : 0);

        m_collider.enabled = t_enable;
        m_detectionArea.enabled = t_enable;

    }

}
