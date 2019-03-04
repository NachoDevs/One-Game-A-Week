using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float gravityScale = 5.0f;
    public float movementSmoothTime = .1f;

    private float m_turnSmoothVelocity;

    private Vector3 m_moveDirection;

    private Animator m_animController;
    private CharacterController m_cController;
    private Transform m_cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        m_animController = GetComponentInChildren<Animator>();
        m_cController = GetComponent<CharacterController>();
        m_cameraTransform = GetComponentInChildren<CameraController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        m_moveDirection = new Vector3(Input.GetAxis("Horizontal") * movementSpeed,      // X axis movement
            Physics.gravity.y * gravityScale,                                           // Gravity
            Input.GetAxis("Vertical") * movementSpeed);                                 // Y axis movement

        m_cController.Move(m_moveDirection * Time.deltaTime);       // Apply the movement

        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,
            Mathf.Rad2Deg + m_cameraTransform.eulerAngles.y,
            ref m_turnSmoothVelocity, movementSmoothTime);

        m_animController.SetFloat("playerSpeed", Mathf.Abs(Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical")));
    }
}
