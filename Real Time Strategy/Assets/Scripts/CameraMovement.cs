using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float cameraMovSpeed = 50f;
    [SerializeField]
    float cameraMaxMoveSpeed = 1;

    Vector3 newCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        newCameraPos = Vector3.zero;

        newCameraPos.x += Input.GetAxisRaw("Vertical");
        newCameraPos.z += Input.GetAxisRaw("Vertical");

        newCameraPos.x += Input.GetAxisRaw("Horizontal");
        newCameraPos.z -= Input.GetAxisRaw("Horizontal");

        newCameraPos.x = Mathf.Clamp(newCameraPos.x, -cameraMaxMoveSpeed, cameraMaxMoveSpeed);
        newCameraPos.z = Mathf.Clamp(newCameraPos.z, -cameraMaxMoveSpeed, cameraMaxMoveSpeed);

        transform.position += newCameraPos * Time.deltaTime * cameraMovSpeed;
    }
}
