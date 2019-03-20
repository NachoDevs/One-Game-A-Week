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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ingredient i = collision.GetComponent<Ingredient>();
        if (collision.GetComponent<Ingredient>())
        {
            recipe.Add(i);
        }
    }
}
