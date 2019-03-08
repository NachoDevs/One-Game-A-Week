using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int currAmmo = 6;
    public int maxAmmo = 6;

    public float damage = 10.0f;
    public float range = 100.0f;
    public float fireRate = 2.0f;
    public float impactForce = 50f;

    public Transform barrel;
    public Camera playerCam;

    public ParticleSystem muzzleFlash;

    public GameObject impactEffect;

    private float m_timer;

    private RaycastHit m_hit;

    private CameraController m_playerCamTarget;

    private Animation m_shootAnimation;

    void Start()
    {
        playerCam = transform.parent.GetComponentInChildren<Camera>();
        m_playerCamTarget = playerCam.GetComponent<CameraController>();
        m_shootAnimation = GetComponentInChildren<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (Input.GetButtonUp("Fire1"))
        {
            if(m_timer >= fireRate)
            {
                if(currAmmo > 0)
                {
                    m_timer = .0f;
                    Shoot();
                }
                else
                {
                    StartCoroutine("Reload");
                }
            }
        }
    }

    void Shoot()
    {
        Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out m_hit);
        //Debug.DrawRay(barrel.position, playerCam.transform.forward * 100, Color.green, 5.0f);

        Vector3 shootDirection = m_hit.point - barrel.position;

        --currAmmo;

        muzzleFlash.Play();
        m_shootAnimation.Play();

        // Knockback
        GetComponentInParent<Rigidbody>().AddForce(-GetComponentInParent<Transform>().right * impactForce);

        //Debug.DrawRay(barrel.position, shootDirection * 100, Color.red, 5.0f);
        if (Physics.Raycast(barrel.position, shootDirection, out m_hit, range))
        {
            if(m_hit.transform == GetComponentInParent<PlayerController>().transform)
            {
                return;
            }

            Target target = m_hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage();
            }

            if(m_hit.rigidbody != null)
            {
                m_hit.rigidbody.AddForce(-m_hit.normal * impactForce);
            }

            Destroy(Instantiate(impactEffect, m_hit.point, Quaternion.LookRotation(m_hit.normal)), 5f);
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(4);
        currAmmo = maxAmmo;
    }
}
