using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoPanel : MonoBehaviour
{
    [Header("UI")]
    public Transform ResourcesCostPanel;
    public TextMeshProUGUI buidlingDescription;

    GameManager m_gm;

    // Start is called before the first frame update
    void Start()
    {
        CheckGM();
    }

    public void SetUpPanel(BuildingType t_bType)
    {
        CheckGM();

        int[] costs = new int[4];

        switch (t_bType)
        {
            default:
            case BuildingType.bakery:
                costs[0] = m_gm.buildings[0].GetComponent<Bakery>().woodCost;
                costs[1] = m_gm.buildings[0].GetComponent<Bakery>().stoneCost;
                costs[2] = m_gm.buildings[0].GetComponent<Bakery>().wheatCost;
                costs[3] = m_gm.buildings[0].GetComponent<Bakery>().breadCost;
                buidlingDescription.text = "Produces bread.";
                break;
            case BuildingType.deliveryHouse:
                costs[0] = m_gm.buildings[1].GetComponent<DeliveryHouse>().woodCost;
                costs[1] = m_gm.buildings[1].GetComponent<DeliveryHouse>().stoneCost;
                costs[2] = m_gm.buildings[1].GetComponent<DeliveryHouse>().wheatCost;
                costs[3] = m_gm.buildings[1].GetComponent<DeliveryHouse>().breadCost;
                buidlingDescription.text = "...";
                break;
            case BuildingType.farm:
                costs[0] = m_gm.buildings[2].GetComponent<Farm>().woodCost;
                costs[1] = m_gm.buildings[2].GetComponent<Farm>().stoneCost;
                costs[2] = m_gm.buildings[2].GetComponent<Farm>().wheatCost;
                costs[3] = m_gm.buildings[2].GetComponent<Farm>().breadCost;
                buidlingDescription.text = "Produces wheat.";
                break;
            case BuildingType.lumber:
                costs[0] = m_gm.buildings[3].GetComponent<Lumber>().woodCost;
                costs[1] = m_gm.buildings[3].GetComponent<Lumber>().stoneCost;
                costs[2] = m_gm.buildings[3].GetComponent<Lumber>().wheatCost;
                costs[3] = m_gm.buildings[3].GetComponent<Lumber>().breadCost;
                buidlingDescription.text = "Produces lumber.";
                break;
            case BuildingType.mine:
                costs[0] = m_gm.buildings[4].GetComponent<Mine>().woodCost;
                costs[1] = m_gm.buildings[4].GetComponent<Mine>().stoneCost;
                costs[2] = m_gm.buildings[4].GetComponent<Mine>().wheatCost;
                costs[3] = m_gm.buildings[4].GetComponent<Mine>().breadCost;
                buidlingDescription.text = "Produces stone.";
                break;
            case BuildingType.warehouse:
                costs[0] = m_gm.buildings[5].GetComponent<Warehouse>().woodCost;
                costs[1] = m_gm.buildings[5].GetComponent<Warehouse>().stoneCost;
                costs[2] = m_gm.buildings[5].GetComponent<Warehouse>().wheatCost;
                costs[3] = m_gm.buildings[5].GetComponent<Warehouse>().breadCost;
                buidlingDescription.text = "...";
                break;
            case BuildingType.windmill:
                costs[0] = m_gm.buildings[6].GetComponent<Windmill>().woodCost;
                costs[1] = m_gm.buildings[6].GetComponent<Windmill>().stoneCost;
                costs[2] = m_gm.buildings[6].GetComponent<Windmill>().wheatCost;
                costs[3] = m_gm.buildings[6].GetComponent<Windmill>().breadCost;
                buidlingDescription.text = "Produces wheat.";
                break;
        }

        for (int i = 0; i < m_gm.resources.Count; ++i)
        {
            GameObject panel = Instantiate(m_gm.resourcePanelPrefab, ResourcesCostPanel);
            panel.GetComponentsInChildren<Image>()[1].sprite = m_gm.resources[i];
            panel.GetComponentInChildren<TextMeshProUGUI>().text = costs[i].ToString();
        }
    }

    void CheckGM()
    {
        if (m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
