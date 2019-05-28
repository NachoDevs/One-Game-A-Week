using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isBuilding;

    [HideInInspector]
    public GameObject selectedBuilding;
    [HideInInspector]
    public GameObject toBuildBuilding;

    [Header("UI")]
    public Transform buildingButtonsPanel;
    public GameObject buildingButtonPrefab;

    [Space]

    public List<GameObject> buildings;

    public List<Material> teamMats;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(CreateNewBuilding(BuildingType.HQ, 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.tag == "Terrain")
                {
                    GameObject inst = Instantiate(toBuildBuilding, hit.point, Quaternion.identity);
                    if(Input.GetMouseButtonUp(0))
                    {
                        Instantiate(CreateNewBuilding(inst.GetComponent<Building>().buildingType, inst.GetComponent<Building>().team, hit.point));
                        isBuilding = false;
                        toBuildBuilding = null;
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
                    print("not terrain");
                }
            }
        }
    }

    public void CreateHQ()
    {
        toBuildBuilding = CreateNewBuilding(BuildingType.HQ, 1);
        isBuilding = true;
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
}

public enum BuildingType
{
    HQ,
    collector,
}
