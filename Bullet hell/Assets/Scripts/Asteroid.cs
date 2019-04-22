using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public List<Sprite> sprites;

    PolygonCollider2D m_collider;

    // Start is called before the first frame update
    void Start()
    {
        AsteroidSetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AsteroidSetUp()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        m_collider = GetComponentInChildren<Transform>().gameObject.AddComponent<PolygonCollider2D>();
        m_collider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedGO = collision.transform.parent.gameObject;

        //if(collidedGO.GetComponent<Player>())
        {

        }

    }
}
