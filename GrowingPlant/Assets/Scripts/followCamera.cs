using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour
{
    public Transform tipOfPlant;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x,
                                                                            tipOfPlant.position.y / 2,
                                                                            transform.position.z/*-tipOfPlant.position.y / 5*/), Time.deltaTime);
    }
}
