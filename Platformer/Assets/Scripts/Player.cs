using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Ingredient m_ingredient;

    void Update()
    {
        if (m_ingredient != null)
        {
            if(Input.GetKeyUp(KeyCode.E))
            {
                print("out");
                m_ingredient.StartCoroutine("EnableIngredientColliders", true);

                m_ingredient.GetComponent<Rigidbody2D>().AddForce(new Vector2(200.5f, 200.5f));

                m_ingredient = null;
            }
            else
            {
                m_ingredient.transform.position = new Vector3(gameObject.transform.position.x
                                                            , gameObject.transform.position.y + 1
                                                            , gameObject.transform.position.z - 1);
            }

        }
    }

    public void TakeIngredient(Ingredient t_ingredient)
    {
        if(m_ingredient == null)
        {
            m_ingredient = t_ingredient;
            m_ingredient.StartCoroutine("EnableIngredientColliders", false);
        }
    }

}
