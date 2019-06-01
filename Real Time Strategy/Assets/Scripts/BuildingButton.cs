using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    static GameManager m_gm;

    bool m_enabled;

    Button m_button;

    // Update is called once per frame
    void Update()
    {
        //m_button.interactable = m_enabled;
    }

    public void SetButtonFunctionalityForUnits(UnitType t_type, Vector3 t_spawnPos)
    {
        if (m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        if (m_button == null)
        {
            m_button = GetComponent<Button>();
        }

        m_button.onClick.AddListener(delegate 
        {
            int unitCost = 0;
            switch (t_type)
            {
                default:
                case UnitType.builder:
                    unitCost = 15;
                    break;
                case UnitType.soldier:
                    unitCost = 100;
                    break;
            }

            if (m_gm.greenAmount < unitCost)
            {
                return;
            }

            m_gm.greenAmount -= unitCost;

            m_gm.selectedBuilding.GetComponent<Building>().QueueNewUnit(t_type, 1, t_spawnPos);
        });
    }

    public void SetButtonFunctionalityForBuildings(BuildingType t_type)
    {
        if (m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        if (m_button == null)
        {
            m_button = GetComponent<Button>();
        }

        m_button.onClick.AddListener(delegate
        {
            int buildCost = 0;
            switch (t_type)
            {
                default:
                case BuildingType.HQ:
                    buildCost = 500;
                    break;
                case BuildingType.collector:
                    buildCost = 75;
                    break;
                case BuildingType.barracks:
                    buildCost = 150;
                    break;
            }

            if (m_gm.greenAmount < buildCost)
            {
                return;
            }

            m_gm.greenAmount -= buildCost;

            m_gm.toBuildBuilding = m_gm.CreateNewBuilding(t_type, 1); m_gm.isBuilding = true;
        });
    }
}
