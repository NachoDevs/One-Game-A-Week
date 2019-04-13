using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    public List<string> toSay;

    [SerializeField]
    TextMeshProUGUI m_speechText;

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
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void Talk(string t_speech)
    {
        gameObject.SetActive(true);

        m_speechText.text = t_speech;
    }
}
