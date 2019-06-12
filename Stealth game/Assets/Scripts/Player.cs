using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool movingSlow;
    public bool isDetected;

    public int walkSpeed = 5;
    public int slowSpeed = 2;

    float movementSpeed;

    SphereCollider attackHitbox;

    // Start is called before the first frame update
    void Start()
    {
        attackHitbox = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAbilities();
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

    void HandleAbilities()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(!isDetected)
            {
                attackHitbox.enabled = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<Enemy>())
        {
            Destroy(other.transform.parent.gameObject);
        }
        attackHitbox.enabled = false;
    }
}
