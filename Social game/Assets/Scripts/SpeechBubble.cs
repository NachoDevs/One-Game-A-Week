using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    public List<string> toSay;

    int speechIndex = 0;    // Used to track the conversation state

    [SerializeField]
    TextMeshProUGUI m_speechText;

    [SerializeField]
    Player m_player;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponentInParent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponentInParent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem)
            {
                //Set the Pointer Event Position to that of the mouse position
                position = Input.mousePosition
            };

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if(result.gameObject == gameObject)
                {
                    if(toSay.Count < (speechIndex + 1))
                    {
                        gameObject.SetActive(false);
                        speechIndex = 0;
                        toSay.Clear();
                        m_player.isTalking = false;
                        m_player.talkingTo.isTalking = false;
                    }
                    else
                    {
                        WriteSpeech(toSay[speechIndex]);
                        ++speechIndex;
                    }
                }
            }
        }
    }

    public void AddToSay(string t_speech)
    {
        toSay.Add(t_speech);

        gameObject.SetActive(true);

        if(speechIndex <= 0)
        {
            WriteSpeech(toSay[speechIndex]);
            ++speechIndex;
        }
    }

    void WriteSpeech(string t_speech)
    {
        m_speechText.text = t_speech;
    }
}
