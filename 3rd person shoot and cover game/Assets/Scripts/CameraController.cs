using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool lockCursor = true;

    public float mouseSensitivity = 5.0f;
    public float dstFromTarget = 1.5f;
    public float cameraSmoothTime = .1f;

    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public Transform target;

    [HideInInspector]
    public float m_yaw;
    [HideInInspector]
    public float m_pitch;

    private Vector3 m_currentRotation;
    private Vector3 m_rotationSmoothVelocity;

    //private Camera m_playerCamera;

    void Start()
    {
        //m_playerCamera.GetComponent<Camera>();
        LockCursor(lockCursor);

        m_pitch = 7;
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            lockCursor = !lockCursor;
            LockCursor(lockCursor);
        }
    }

    void LateUpdate()
    {
        m_yaw += Input.GetAxis("Mouse X");
        m_pitch -= Input.GetAxis("Mouse Y");
        m_pitch = Mathf.Clamp(m_pitch, pitchMinMax.x, pitchMinMax.y);

        m_currentRotation = Vector3.SmoothDamp(m_currentRotation, new Vector3(m_pitch, m_yaw), ref m_rotationSmoothVelocity, cameraSmoothTime);

        transform.eulerAngles = m_currentRotation;

        transform.position = target.position - transform.forward * dstFromTarget;
    }

    void LockCursor(bool t_lockCursor)
    {
        Cursor.lockState = (t_lockCursor) ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !t_lockCursor;
    }
}
