using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    public Tree tree;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<Player>() != null)
        {
            Player p = other.GetComponentInParent<Player>();
            if (Input.GetKeyUp(KeyCode.E))
            {
                if(enabled)
                {
                    if(p.carrying == null)
                    {
                        p.carrying = gameObject;
                        gameObject.GetComponent<BoxCollider>().enabled = false;
                        gameObject.GetComponent<SphereCollider>().enabled = false;
                        transform.parent = p.transform;
                        tree.fruits.Remove(gameObject);
                    }
                }
            }
        }
    }
}
