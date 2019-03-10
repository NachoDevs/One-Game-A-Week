using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public bool canBeUsed = true;

    public GameObject ammoIcon;
    
    public BoxCollider collider;
    
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!canBeUsed)
        {
            return;
        }

        if(other.GetComponentInChildren<Gun>() != null)
        {
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
