using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingTypeBase : MonoBehaviour
{
    protected static GameManager m_gm;

    protected Building assignedBuilding;

    RaycastHit hit;


    // Start is called before the first frame update
    protected void Start()
    {
        if(m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        assignedBuilding = GetComponent<Building>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (!assignedBuilding.clicked)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (hit.transform.parent.GetComponent<Building>() == GetComponent<Building>())
                    {
                        assignedBuilding.clicked = true;
                        m_gm.selectedBuilding = hit.transform.parent.gameObject;
                        if (hit.transform.parent.GetComponent<Building>().owned)
                        {
                            LeftClick();
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    assignedBuilding.clicked = false;
                }
            }
        }
    }

    protected virtual void LeftClick()
    {
        print("left click");
    }
}
