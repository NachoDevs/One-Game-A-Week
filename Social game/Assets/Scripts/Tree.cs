using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    public GameObject fruit;

    public List<GameObject> fruitSpawn;

    bool m_canSpawn;

    public List<GameObject> fruits;

    // Start is called before the first frame update
    void Start()
    {
        fruits = new List<GameObject>();

        StartCoroutine(SpawnNewFruits(0));

        InvokeRepeating("CanSpawn", 1f, 1f);
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
            GameObject fruitGO = Instantiate(fruit, go.transform);
            fruitGO.GetComponent<Fruit>().tree = this;
            fruits.Add(fruitGO);

        }

        m_canSpawn = false;
    }

    void CanSpawn()
    {
        if(fruits.Count <= 0)
        {
            m_canSpawn = true;
            StartCoroutine(SpawnNewFruits(2));
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.GetComponentInParent<Player>() != null)
        {
            if(m_canSpawn)
            {
                return;
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                foreach (GameObject go in fruits)
                {
                    go.GetComponent<Rigidbody>().useGravity = true;
                    go.GetComponent<Fruit>().enabled = true;
                }
            }
        }
    }
}
