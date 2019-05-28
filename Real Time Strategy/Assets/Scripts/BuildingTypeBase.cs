using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeBase : MonoBehaviour
{
    public List<BuildingType> canBuild;

    protected static GameManager m_gm;

    protected Building m_assignedBuilding;

    RaycastHit hit;


    // Start is called before the first frame update
    protected void Start()
    {
        if(m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        m_assignedBuilding = GetComponent<Building>();

        canBuild = new List<BuildingType>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if(!m_gm.isBuilding)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (!m_assignedBuilding.clicked)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (hit.transform.parent.GetComponent<Building>() == GetComponent<Building>())
                        {
                            m_assignedBuilding.clicked = true;
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
                        m_assignedBuilding.clicked = false;
                    }
                }
            }
        }

    }

    protected virtual void LeftClick()
    {
        foreach (Transform child in m_gm.buildingButtonsPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (BuildingType bt in canBuild)
        {
            GameObject go = Instantiate(m_gm.buildingButtonPrefab, m_gm.buildingButtonsPanel);
            go.GetComponentInChildren<Text>().text = bt.ToString();
            go.GetComponent<BuildingButton>().SetButtonFunctionality(bt);
        }
    }
}
