using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTalking;

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
            if(!isTalking)
            {
                if(other.GetComponentInParent<NPC>() != null)
                {
                    NPC npc = other.GetComponentInParent<NPC>();
                    isTalking = true;
                    if(npc.waitingForFruit)
                    {
                        if(carrying == null)
                        {
                            npc.Talk("goodbyes");
                        }
                        else
                        {
                            npc.Talk("thanks");

                            npc.IncrementFriendshipLevel((carrying.GetComponent<Fruit>().fruitType == npc.wantedFruit) ? 1 : -1);

                            npc.waitingForFruit = false;

                            Destroy(carrying);
                        }
                    }
                    else
                    {
                        npc.Quest();
                    }
                }
            }
        }
    }
}
