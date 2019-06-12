using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool movingSlow;

    public int walkSpeed = 5;
    public int slowSpeed = 2;

    float movementSpeed;

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
        if(Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = slowSpeed;
            movingSlow = true;
        }
        else
        {
            movementSpeed = walkSpeed;
            movingSlow = false;
        }

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
    }
}
