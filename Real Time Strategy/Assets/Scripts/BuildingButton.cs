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

    public void SetButtonFunctionality(BuildingType t_type)
    {
        if (m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        if (m_button == null)
        {
            m_button = GetComponent<Button>();
        }

        m_button.onClick.AddListener(delegate { m_gm.toBuildBuilding = m_gm.CreateNewBuilding(t_type, 1); m_gm.isBuilding = true; });
    }
}
