using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Camera m_cam;

    RaycastHit m_hit;

    // Start is called before the first frame update
    void Start()
    {
        m_cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out m_hit, Mathf.Infinity))
            {
                try
                {
                    if (m_hit.collider.gameObject.GetComponentInParent<Board>() != null)
                    {
                        m_hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                }
                catch (Exception e) { /*PrintException(e);*/ }
            }
        }
    }

    public static void PrintException(Exception t_e)
    {
        print
            (
            "---------Exception---------\n"
            + "Message: \t" + t_e.Message + "\n"
            + "Source: \t" + t_e.Source + "\n"
            + "Data: \t" + t_e.Data + "\n"
            + "Trace: \t" + t_e.StackTrace + "\n"
            );
    }
}
