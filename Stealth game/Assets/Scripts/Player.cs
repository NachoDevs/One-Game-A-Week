using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isDetected;
    public bool isDragging;
    public bool isMovingSlow;

    public int walkSpeed = 5;
    public int slowSpeed = 2;

    public Vector3 forwardVector;

    float movementSpeed;

    Vector3 dragOffset;

    GameManager m_gm;

    GameObject m_dragging;

    SphereCollider attackHitbox;

    MeshRenderer m_meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        attackHitbox = GetComponent<SphereCollider>();
        m_meshRenderer = GetComponentInChildren<MeshRenderer>();

        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
        if(Input.GetKey(KeyCode.LeftShift) || isDragging)
        {
            movementSpeed = slowSpeed;
            isMovingSlow = true;
        }
        else
        {
            movementSpeed = walkSpeed;
            isMovingSlow = false;
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
        if(Input.GetKeyUp(KeyCode.E))
        {
            if(isDragging)
            {
                isDragging = false;
                m_dragging = null;
                return;
            }

            if (!isDetected)
            {
                attackHitbox.enabled = true;
            }
        }

        if(isDragging)
        {
            Drag();
            print("dragging");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<Enemy>())
        {
            if(!other.GetComponentInParent<Enemy>().isDead)
            {
                other.GetComponentInParent<Enemy>().Die();
            }
            else
            {
                m_dragging = other.transform.parent.gameObject;
                dragOffset = transform.position - m_dragging.transform.position;
                isDragging = true;

            }
        }
        attackHitbox.enabled = false;
    }

    void Drag()
    {
        m_dragging.transform.position = transform.position - dragOffset;
    }
}
