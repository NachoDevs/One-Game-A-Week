using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTalking;

    public GameObject carrying;

    public NPC talkingTo;

    GameManager m_gm;

    SphereCollider interactionArea;

    // Start is called before the first frame update
    void Start()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTalking)
        {
            GetComponent<Pathfinding>().agent.SetDestination(transform.position);
        }

        if (carrying != null)
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
                    npc.isTalking = true;
                    talkingTo = npc;
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
                        if(npc.questReady)
                        {
                            npc.Quest();
                        }
                        else
                        {
                            npc.ChitChat();
                        }
                    }
                }
            }
        }
    }

    public void AcceptConfirmation()
    {
        talkingTo.Talk("thanks");
        m_gm.ConfirmationPanel.SetActive(false);

        talkingTo.IncrementFriendshipLevel(0);
    }

    public void DenyConfirmation()
    {
        talkingTo.waitingForFruit = false;

        talkingTo.Talk("goodbyes");
        m_gm.ConfirmationPanel.SetActive(false);

        talkingTo.IncrementFriendshipLevel(-1);
    }
}
