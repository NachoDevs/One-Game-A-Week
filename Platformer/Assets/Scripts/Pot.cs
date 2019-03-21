using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{

    public List<Ingredient> recipe;

    GameManager m_gm;

    // Start is called before the first frame update
    void Awake()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Ingredient i = collision.GetComponentInParent<Ingredient>();
        if (i != null)
        {
            Sprite sp = i.GetComponentInChildren<SpriteRenderer>().sprite;

            if (m_gm.roundRecipe.ContainsKey(sp))
            {
                if(m_gm.roundRecipe[sp] > 0)
                {
                    --m_gm.roundRecipe[sp];
                    Destroy(i.gameObject);
                    return;
                }
            }
            collision.GetComponentInParent<Rigidbody2D>().AddForce(new Vector2(500f, 500f));
        }
    }
}
