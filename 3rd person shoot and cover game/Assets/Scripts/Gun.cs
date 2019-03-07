using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10.0f;
    public float range = 100.0f;
    public float fireRate = 2.0f;

    public Transform barrel;
    public Transform aimDot;
    public Camera playerCam;

    private float m_timer;

    private RaycastHit hit;

    private CameraController playerCamTarget;

    void Start()
    {
        playerCam = transform.parent.GetComponentInChildren<Camera>();
        playerCamTarget = playerCam.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {

        aimDot.position = new Vector3(aimDot.position.x, 3 + -playerCamTarget.m_pitch * 0.1f, aimDot.position.z);

        m_timer += Time.deltaTime;
        if(m_timer >= fireRate)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                m_timer = .0f;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector3 shootDirection = barrel.position - (playerCam.transform.position - aimDot.position);

        Debug.DrawRay(barrel.position, barrel.forward * 100, Color.red, 5.0f);
        if(Physics.Raycast(barrel.position, shootDirection, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
