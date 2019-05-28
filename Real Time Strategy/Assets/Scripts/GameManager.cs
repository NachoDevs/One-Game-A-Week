using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject selectedBuilding;

    public List<GameObject> buildings;

    public List<Material> teamMats;

    // Start is called before the first frame update
    void Start()
    {
        CreateNewBuilding(BuildingType.HQ, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject CreateNewBuilding(BuildingType t_type, int t_team, Vector3 t_pos = new Vector3())
    {
        GameObject building;

        switch (t_type)
        {
            default:
            case BuildingType.HQ:
                building = Instantiate(buildings[0]);
                break;
            case BuildingType.collector:
                building = Instantiate(buildings[1]);
                break;
        }

        building.transform.position = t_pos;

        building.GetComponent<Building>().team = t_team;
        building.GetComponent<Building>().buildingType = t_type;

        return building;
    }
}

public enum BuildingType
{
    HQ,
    collector,
}
