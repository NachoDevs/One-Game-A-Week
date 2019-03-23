using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public GameObject ingredientPrefab;

    public SpriteRenderer ingredientSprite;

    public Sprite ingredient;

    Vector3 m_spawnPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<CharacterController2D>() != null)
        {
            m_spawnPos = new Vector3( transform.position.x
                                    , gameObject.transform.position.y + 2.5f
                                    , gameObject.transform.position.z );

            GameObject go = Instantiate(ingredientPrefab, m_spawnPos, transform.rotation, transform);
            go.GetComponent<Ingredient>().ingredientSprite = ingredient;
            go.GetComponent<Ingredient>().GetComponentInChildren<SpriteRenderer>().sprite = ingredient;
        }
    }

}
