using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float asteroidSpeed = 10;
    public float rotateSpeed = 25;

    public List<Sprite> sprites;

    PolygonCollider2D m_collider;

    // Start is called before the first frame update
    void Start()
    {
        AsteroidSetUp();

        GetComponentInChildren<Rigidbody2D>().AddForce((GameObject.FindGameObjectsWithTag("Player")[0].transform.position - transform.position) * asteroidSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime, Space.Self);
    }

    void AsteroidSetUp()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        m_collider = GetComponentInChildren<Transform>().gameObject.AddComponent<PolygonCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedGO = collision.transform.parent.gameObject;
    }
}
