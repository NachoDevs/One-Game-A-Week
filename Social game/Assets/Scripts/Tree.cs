using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    public GameObject fruit;

    public List<GameObject> fruitSpawn;

    List<GameObject> fruits;

    // Start is called before the first frame update
    void Start()
    {
        fruits = new List<GameObject>();

        StartCoroutine(SpawnNewFruits(0));
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnNewFruits(int t_seconds)
    {
        yield return new WaitForSeconds(t_seconds);

        foreach (GameObject go in fruitSpawn)
        {
            fruits.Add(Instantiate(fruit, go.transform));
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.GetComponentInParent<Player>() != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                foreach (GameObject go in fruits)
                {
                    go.GetComponent<Rigidbody>().useGravity = true;
                    //go.GetComponent<Fruit>().
                }
                fruits.Clear();
                StartCoroutine(SpawnNewFruits(2));
            }
        }
    }
}
