using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class plant : MonoBehaviour
{
    public float growingRate = .001f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, 
                                                                                transform.localScale.y,
                                                                                transform.localScale.z + growingRate), Time.deltaTime);

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x + (growingRate * .1f),
                                                                                transform.localScale.y + (growingRate * .1f),
                                                                                transform.localScale.z), Time.deltaTime);
        
    }
}
