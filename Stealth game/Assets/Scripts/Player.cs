using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(movementSpeed * Time.deltaTime,
                                    0,
                                    movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-movementSpeed * Time.deltaTime,
                                                0,
                                                movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(-movementSpeed * Time.deltaTime,
                                                0,
                                                -movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(movementSpeed * Time.deltaTime,
                                0,
                                -movementSpeed * Time.deltaTime);
        }

        // Boundaries, to be improved
        if (transform.position.x <= -65)
        {
            transform.position = new Vector3(-60, transform.position.y, transform.position.z);
        }
        if (transform.position.x >= 35)
        {
            transform.position = new Vector3(30, transform.position.y, transform.position.z);
        }
        if (transform.position.z <= -35)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -30);
        }
        if (transform.position.z >= 20)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 16);
        }
    }
}
