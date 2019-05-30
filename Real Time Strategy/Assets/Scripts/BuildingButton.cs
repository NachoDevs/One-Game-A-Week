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
            m_gm.toBuildBuilding = m_gm.CreateNewBuilding(t_type, 1); m_gm.isBuilding = true;
        });
    }
}
