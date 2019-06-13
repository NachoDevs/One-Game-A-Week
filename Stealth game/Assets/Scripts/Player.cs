using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool movingSlow;
    public bool isDetected;

    public int walkSpeed = 5;
    public int slowSpeed = 2;

    public Vector3 forwardVector;

    float movementSpeed;

    SphereCollider attackHitbox;

    MeshRenderer m_meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        attackHitbox = GetComponent<SphereCollider>();
        m_meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleLookDirection();
        HandleMovement();
        HandleAbilities();
    }

    void HandleLookDirection()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Vector3 pointToLook = hit.point;
            pointToLook.y = .55f;
            pointToLook -= m_meshRenderer.transform.position;
            m_meshRenderer.transform.rotation = Quaternion.LookRotation(pointToLook, Vector3.up);
            forwardVector = m_meshRenderer.transform.forward;
        }
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
