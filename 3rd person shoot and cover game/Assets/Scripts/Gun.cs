using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool m_isReloading = false;

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

    public TextMeshProUGUI ammoIndicator;

    public AudioSource shotAudio;
    public AudioSource reloadAudio;

    private float m_timer;

    private RaycastHit m_hit;

    private CameraController m_playerCamTarget;

    private Animator m_shootAnimationController;

    private PlayerController m_player;

    private float[] m_shootSoundTime= {.5f, 3.1f, 5.5f, 7.6f, 9.9f, 12.3f, 14.8f, 17.1f, 20.0f, 22.9f};

    void Start()
    {
        m_player = transform.parent.GetComponent<PlayerController>();
        playerCam = transform.parent.GetComponentInChildren<Camera>();
        m_playerCamTarget = playerCam.GetComponent<CameraController>();
        m_shootAnimationController = GetComponentInChildren<Animator>();
        shotAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if(m_player.canInput)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                if(!m_isReloading)
                {
                    if (m_timer >= fireRate)
                    {
                        if (currAmmo > 0)
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

            if (Input.GetKeyUp(KeyCode.R))
            {
                if (!m_isReloading)
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

        IncrementAmmo(currAmmo-1);

        muzzleFlash.Play();
        m_shootAnimationController.SetTrigger("Shooting");
        StartCoroutine("PlayRandomShot");

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
                m_hit.rigidbody.AddForce(-m_hit.normal * impactForce * 50);
            }

            PlayerController targetPC = m_hit.transform.GetComponent<PlayerController>();
            if(targetPC != null)
            {
                targetPC.canInput = false;
            }

            Destroy(Instantiate(impactEffect, m_hit.point, Quaternion.LookRotation(m_hit.normal)), 5f);
        }
    }

    public void IncrementAmmo(int t_ammo)
    {
        currAmmo = t_ammo;
        ammoIndicator.text = t_ammo + " / " + maxAmmo;
    }

    IEnumerator Reload()
    {
        reloadAudio.Play();
        m_isReloading = true;
        m_shootAnimationController.SetBool("Reloading", true);
        yield return new WaitForSeconds(4);
        IncrementAmmo(maxAmmo);
        m_isReloading = false;
        m_shootAnimationController.SetBool("Reloading", false);
        reloadAudio.Stop();
    }

    IEnumerator PlayRandomShot()
    {
        int index = Random.Range(0, 9);
        shotAudio.Play();
        shotAudio.time = m_shootSoundTime[index];
        yield return new WaitForSeconds(.9f);
        shotAudio.Stop();
    }
}
