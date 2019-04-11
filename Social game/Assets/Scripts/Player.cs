using UnityEngine;

public class Player : MonoBehaviour
{

    SphereCollider interactionArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(other.GetComponentInParent<NPC>() != null)
            {
                other.GetComponentInParent<NPC>().ShowLove();
            }
        }
    }
}
