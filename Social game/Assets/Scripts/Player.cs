using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject carrying;

    SphereCollider interactionArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(carrying != null)
        {
            carrying.transform.position = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(other.GetComponentInParent<NPC>() != null)
            {
                other.GetComponentInParent<NPC>().ShowLove();
            }
        }
    }
}
