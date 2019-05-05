using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float shakeDuration = .5f;
    public float shakeAmplitude = 1.5f;
    public float shakeFrequency = 2.0f;

    public GameObject blockEffect;

    public BoxCollider2D meleeCollider;
    public PolygonCollider2D specialCollider;

    public Cinemachine.CinemachineVirtualCamera cVirtualCamera;

    internal Animator m_animator;

    internal CharacterController2D m_controller;

    internal PlayerInputManager m_pInput;

    internal Rigidbody2D m_rb;

    internal GameManager m_gm;

    internal Cinemachine.CinemachineBasicMultiChannelPerlin m_vcNoise;

    float shakeElapsedTime = 0f;

    void Awake()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        m_controller = GetComponent<CharacterController2D>();
        m_rb = GetComponent<Rigidbody2D>();
        m_pInput = GetComponent<PlayerInputManager>();
        m_animator = GetComponentInChildren<Animator>();

        if (cVirtualCamera != null)
        {
            m_vcNoise = cVirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }

    void Update()
    {
        if (cVirtualCamera != null && m_vcNoise != null)
        {
            if(shakeElapsedTime > 0)
            {
                m_vcNoise.m_AmplitudeGain = shakeAmplitude;
                m_vcNoise.m_FrequencyGain = shakeFrequency;

                shakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                m_vcNoise.m_AmplitudeGain = .0f;
                shakeElapsedTime = .0f;
            }
        }
    }

    public void MeleeDamaged()
    {
        if (m_pInput.m_isBlocking)
        {
            m_pInput.m_isBlocking = false;
            m_pInput.m_blockTimer = 0;
            Vector3 blockPos = transform.position;
            blockPos.y -= 0.2f;
            Destroy(Instantiate(blockEffect, blockPos, Quaternion.identity), .35f);
        }
        else
        {
            StartCoroutine(Die());
        }
    }

    public void SpecialDamaged(GameObject t_target)
    {
        CameraShake();

        if (m_pInput.m_isBlocking)
        {
            t_target.GetComponent<Rigidbody2D>().AddForce(new Vector2(10f * ((m_controller.isFacingRight) ? 1 : -1), 200f));
        }

        MeleeDamaged();
    }

    IEnumerator Die()
    {
        m_controller.m_canMove = false;
        m_animator.SetTrigger("dead");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);

    }

    void CameraShake()
    {
        shakeElapsedTime = shakeDuration;
    }
}
