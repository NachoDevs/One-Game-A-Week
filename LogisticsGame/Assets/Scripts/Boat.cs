using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : Vehicle
{
    // Start is called before the first frame update
    void Start()
    {
        maxFuel = currentFuel = 1250;
        name = "boat_" + boatNumber;
        ++boatNumber;

        maxCapacity = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override List<Vector3> FindPath(City t_destination)
    {
        List<Vector3> path = new List<Vector3>();

        if (t_destination.onCoast && t_destination.cityType == CityType.harbor)
        {
            path.Add(currentCity.transform.position);
            path.Add(t_destination.transform.position);
        }

        return path;
    }

    public override float CalculateFuelComsumption(List<Vector3> path)
    {
        return .0f;
    }
}
