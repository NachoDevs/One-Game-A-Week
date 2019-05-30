using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeBase : MonoBehaviour
{
    public List<UnitType> canBuild;

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

        canBuild = new List<UnitType>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if(!m_gm.isBuilding)
        {
            Ray a = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
            if (Physics.Raycast(a, out hit))
            {
                //Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward * 50);

                if (!m_assignedBuilding.clicked)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        try
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
                        catch (Exception e) { GameManager.PrintException(e); m_gm.selectedBuilding = null; ClearUnitsPanel(); }
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
        ClearUnitsPanel();

        m_gm.unitsButtonsPanel.gameObject.SetActive(true);

        foreach (UnitType ut in canBuild)
        {
            Vector3 spawnPos = Vector3.zero;
            spawnPos.x = -transform.position.x - transform.localScale.x;
            spawnPos.z = -transform.position.z - transform.localScale.z;

            GameObject go = Instantiate(m_gm.buttonPrefab, m_gm.unitsButtonsPanel);
            go.GetComponentInChildren<Text>().text = ut.ToString();
            go.GetComponent<BuildingButton>().SetButtonFunctionalityForUnits(ut, spawnPos);
        }
    }

    void ClearUnitsPanel()
    {
        foreach (Transform child in m_gm.unitsButtonsPanel)
        {
            Destroy(child.gameObject);
        }

        m_gm.unitsButtonsPanel.gameObject.SetActive(false);
    }
}
