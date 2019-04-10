using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraMovementeSpeed = 10f;
    public float cameraZoomSpeed = 100f;

    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        MovementChecks();
        ZoomChecks();

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = player.transform.position;
        }
    }

    void MovementChecks()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(cameraMovementeSpeed * Time.deltaTime,
                                    0,
                                    cameraMovementeSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-cameraMovementeSpeed * Time.deltaTime,
                                                0,
                                                cameraMovementeSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(-cameraMovementeSpeed * Time.deltaTime,
                                                0,
                                                -cameraMovementeSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(cameraMovementeSpeed * Time.deltaTime,
                                0,
                                -cameraMovementeSpeed * Time.deltaTime);
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

    void ZoomChecks()
    {
        if (Input.mouseScrollDelta.y != 0) // Zoom In/Out
        {
            float oSize = GetComponentInChildren<Camera>().orthographicSize;
            oSize -= Input.mouseScrollDelta.y * cameraZoomSpeed * Time.deltaTime;

            // Boundaries
            if (oSize <= 5)
            {
                oSize = 5;
            }
            else if (oSize >= 15)
            {
                oSize = 15;
            }

            GetComponentInChildren<Camera>().orthographicSize = oSize;
        }
    }
}

