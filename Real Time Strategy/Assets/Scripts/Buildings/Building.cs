using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool owned;
    public bool clicked;
    public bool isBuilded;

    public int team;

    public BuildingType buildingType;

    static GameManager m_gm;

    Renderer m_rend;

    // Start is called before the first frame update
    void Start()
    {
        if(m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        m_rend = GetComponentInChildren<Renderer>();

        TeamSetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TeamSetUp()
    {
        Material teamMat;
        string teamName;
        switch(team)
        {
            default:
                teamMat = m_gm.teamMats[0];
                teamName = "neutral";
                break;
            case 1:
                teamMat = m_gm.teamMats[1];
                teamName = "red";
                break;
            case 2:
                teamMat = m_gm.teamMats[2];
                teamName = "blue";
                break;
        }
        m_rend.material = teamMat;
    
        // Finding if the parent for this team has already been created
        bool parentFound = false;
        foreach ( GameObject teamParent in GameObject.FindGameObjectsWithTag("TeamParent"))
        {
            if (teamParent.name == "teamParent_" + teamName)
            {
                transform.parent = teamParent.transform;
                parentFound = true;
                break;
            }
        }
        // If not found, create it
        if(!parentFound)
        {
            GameObject tParent = new GameObject
            {
                name = "teamParent_" + teamName,
                tag = "TeamParent"
            };

            transform.parent = tParent.transform;
        }
    }

    public void QueueNewUnit(UnitType t_type, int t_team, Vector3 t_pos = new Vector3())
    {
        //int unitCost = 0;
        GameObject unit;

        switch (t_type)
        {
            default:
            case UnitType.builder:
                unit = m_gm.units[0];
                //unitCost = 15;
                break;
            case UnitType.soldier:
                unit = m_gm.units[1];
                //unitCost = 100;
                break;
        }

        //m_gm.greenAmount -= unitCost;
        m_gm.greenText.text = m_gm.greenAmount.ToString();

        unit.GetComponent<Unit>().team = t_team;
        unit.GetComponent<Unit>().unitType = t_type;

        Instantiate(unit);
        unit.transform.position = t_pos;
    }
}
