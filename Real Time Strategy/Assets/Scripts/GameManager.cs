using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> buildings;

    public List<Material> teamMats;

    // Start is called before the first frame update
    void Start()
    {
        GameObject building = Instantiate(buildings[0]);
        building.GetComponent<Building>().team = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum BuildingType
{
    HQ,
}
