using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public GameObject ingredientPrefab;

    public Transform ingredientesParent;

    Vector3 m_spawnPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<CharacterController2D>() != null)
        {
            m_spawnPos = new Vector3( collision.gameObject.transform.position.x
                                    , collision.gameObject.transform.position.y + 2.5f
                                    , collision.gameObject.transform.position.z );

            Instantiate(ingredientPrefab, m_spawnPos, collision.gameObject.transform.rotation, ingredientesParent);
        }
    }

}
