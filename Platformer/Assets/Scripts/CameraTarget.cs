using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public int cameraExtraRelativeHeight = 1;

    public float cameraSmoothTime = .1f;

    public GameObject player;

    Vector3 m_rotationSmoothVelocity;
    Vector3 m_pos;

    Rigidbody2D m_playerRigid;

    void Awake()
    {
        m_pos = new Vector3();
        m_playerRigid = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_pos.y = player.transform.position.y + cameraExtraRelativeHeight;
        transform.position = Vector3.SmoothDamp(transform.position, m_pos, ref m_rotationSmoothVelocity, cameraSmoothTime); ;
    }

    void FixedUpdate()
    {
        //cameraExtraRelativeHeight = (m_playerRigid.velocity.y < 0) ? -1 : 1;
    }
}
