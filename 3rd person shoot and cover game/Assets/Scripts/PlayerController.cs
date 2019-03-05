using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 5.0f;
    public float runningSpeed = 10.0f;
    public float gravityScale = 5.0f;
    public float movementSmoothTime = .1f;
    public float speedSmoothTime = .1f;

    private bool m_isRunning;

    private float m_currSpeed;
    private float m_turnSmoothVelocity;
    private float m_speedSmoothVelocity;

    private Vector3 m_moveDirection;

    private Animator m_animController;
    private CharacterController m_cController;
    private Transform m_cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        m_animController = GetComponentInChildren<Animator>();
        m_cameraTransform = GetComponentInChildren<CameraController>().transform;
        m_cController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        m_isRunning = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((m_isRunning) ? runningSpeed : walkingSpeed);
        m_currSpeed = Mathf.SmoothDamp(m_currSpeed, targetSpeed, ref m_speedSmoothVelocity, speedSmoothTime);

        m_moveDirection = new Vector3(Input.GetAxis("Horizontal") * m_currSpeed,      // X axis movement
            Physics.gravity.y * gravityScale,                                         // Gravity
            Input.GetAxis("Vertical") * m_currSpeed);                                 // Y axis movement

        m_cController.Move(m_moveDirection * Time.deltaTime);       // Apply the movement

        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,
            m_cameraTransform.eulerAngles.y,
            ref m_turnSmoothVelocity, movementSmoothTime);

        m_animController.SetFloat("playerSpeed", Input.GetAxis("Horizontal") + Input.GetAxis("Vertical"));
    }
}
