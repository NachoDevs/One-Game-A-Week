using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public bool canBeUsed = true;

    public GameObject ammoIcon;
      
    public ParticleSystem ps;

    private AudioSource m_pickupSound;

    // Start is called before the first frame update
    void Start()
    {
        m_pickupSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!canBeUsed)
        {
            return;
        }

        if(other.GetComponentInChildren<Gun>() != null)
        {
            m_pickupSound.Play();
            other.GetComponentInChildren<Gun>().IncrementAmmo(other.GetComponentInChildren<Gun>().maxAmmo);
            StartCoroutine("PickedUp");
        }
    }

    IEnumerator PickedUp()
    {
        canBeUsed = false;
        ammoIcon.SetActive(false);
        ps.Stop(true);
        yield return new WaitForSeconds(5f);
        canBeUsed = true;
        ammoIcon.SetActive(true);
        ps.Play(true);
    }
}
