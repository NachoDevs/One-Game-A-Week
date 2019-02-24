using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform tipOfPlant;

    public float cameraSpeed = .5f;

    // Update is called once per frame
    void Update()
    {
        // Input handeling for the camera
        switch (Input.inputString)
        {
            case "w":   // Going up
                Vector3 p = transform.position;
                transform.position = new Vector3(p.x, p.y += cameraSpeed, p.z);
                if (p.y >= tipOfPlant.transform.position.y)
                {
                    transform.position = new Vector3(p.x, tipOfPlant.transform.position.y, p.z);
                }
                break;
            case "s":   // Going down
                p = transform.position;
                transform.position = new Vector3(p.x, p.y -= cameraSpeed, p.z);
                if(p.y <= 6.5f)
                {
                    transform.position = new Vector3(p.x, 6.5f, p.z);
                }
                break;
            default:
                if (Input.mouseScrollDelta.y != 0) // Zoom In/Out
                {
                    p = transform.position;
                    transform.position = new Vector3(p.x, p.y, p.z += Input.mouseScrollDelta.y);
                    if (p.z >= 5)
                    {
                        transform.position = new Vector3(p.x, p.y, 5);
                    }
                }
                break;
        }
    }
}
