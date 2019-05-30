using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isBuilding;

    public int greenAmount;
    public int initialGreen = 25;

    [HideInInspector]
    public GameObject selectedBuilding;
    [HideInInspector]
    public GameObject toBuildBuilding;

    [Header("UI")]
    public Transform buildingButtonsPanel;
    public Transform unitsButtonsPanel;
    public GameObject buttonPrefab;
    public TextMeshProUGUI greenText;

    [Space]

    public List<GameObject> buildings;
    public List<GameObject> resources;
    public List<GameObject> units;
    [HideInInspector]
    public List<GameObject> soldiers;

    public List<Material> teamMats;
    public List<Material> resourceMats;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        GameObject hq = Instantiate(CreateNewBuilding(BuildingType.HQ, 1));
        hq.GetComponent<Building>().isBuilded = true;

        soldiers = new List<GameObject>();

        SetBuildingButtons();

        greenAmount = initialGreen;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f/*, ~(1 << 5)*/))
            {
                if (hit.transform.parent != null)
                {
                    return;
                }

                if (hit.transform.tag == "Terrain")
                {
                    GameObject inst = Instantiate(toBuildBuilding, hit.point, Quaternion.identity);
                    //inst.layer = 2; // Ignore Raycast
                    if (Input.GetMouseButtonUp(0))
                    {
                        GameObject definitive = Instantiate(CreateNewBuilding(inst.GetComponent<Building>().buildingType, inst.GetComponent<Building>().team, hit.point));
                        //inst.layer = 0; // Default 
                        isBuilding = false;
                        toBuildBuilding = null;
                        definitive.GetComponent<Building>().isBuilded = true;
                    }
                    else if(Input.GetMouseButtonUp(1))
                    {
                        isBuilding = false;
                        toBuildBuilding = null;
                    }
                    Destroy(inst, .02f);
                }
                else
                {
                    //print("not terrain");
                }
            }
        }
    }

    public GameObject CreateNewBuilding(BuildingType t_type, int t_team, Vector3 t_pos = new Vector3())
    {
        GameObject building;

        switch (t_type)
        {
            default:
            case BuildingType.HQ:
                building = buildings[0];
                break;
            case BuildingType.collector:
                building = buildings[1];
                break;
        }

        building.transform.position = t_pos;

        building.GetComponent<Building>().team = t_team;
        building.GetComponent<Building>().buildingType = t_type;


        building.GetComponent<Building>().owned = true;

        return building;
    }

    void SetBuildingButtons()
    {
        foreach (Transform child in buildingButtonsPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (GameObject bt in buildings)
        {
            GameObject go = Instantiate(buttonPrefab, buildingButtonsPanel);
            go.GetComponentInChildren<Text>().text = bt.GetComponent<Building>().buildingType.ToString();
            go.GetComponent<BuildingButton>().SetButtonFunctionalityForBuildings(bt.GetComponent<Building>().buildingType);
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

public enum BuildingType
{
    HQ,
    collector,
}

public enum UnitType
{
    builder,
    soldier,
}

public enum ResourceType
{
    greeResource,
}

public enum AIIntentions
{
    idle,
    collectResources,
    deliverResources,
}
