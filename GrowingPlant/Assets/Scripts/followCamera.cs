using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform tipOfPlant;

    public float cameraSpeed = .5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (Input.inputString)
        {
            case "w":
                Vector3 p = transform.position;
                transform.position = new Vector3(p.x, p.y += cameraSpeed, p.z);
                if (p.y >= tipOfPlant.transform.position.y)
                {
                    transform.position = new Vector3(p.x, tipOfPlant.transform.position.y, p.z);
                }
                break;
            case "s":
                p = transform.position;
                transform.position = new Vector3(p.x, p.y -= cameraSpeed, p.z);
                if(p.y <= 6.5f)
                {
                    transform.position = new Vector3(p.x, 6.5f, p.z);
                }
                break;
            default:
                if (Input.mouseScrollDelta.y != 0)
                {
                    p = transform.position;
                    transform.position = new Vector3(p.x, p.y, p.z += Input.mouseScrollDelta.y);
                }

                    break;
        }

        //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x,
        //                                                                    tipOfPlant.position.y / 2,
        //                                                                    transform.position.z -tipOfPlant.position.y / 3), Time.deltaTime);
    }
}
