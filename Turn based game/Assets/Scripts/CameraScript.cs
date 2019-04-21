using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float dragSpeed = .5f;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxis("Mouse X") * dragSpeed;
            float y = Input.GetAxis("Mouse Y") * dragSpeed;
            transform.Translate(-x, -y, 0);
        }
    }

    public void StartCameraPosition(Vector3 posA, Vector3 posB)
    {
        transform.position = (posA + posB) * .5f;
    }
}
