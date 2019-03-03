using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : Vehicle
{
    // Start is called before the first frame update
    void Start()
    {
        maxFuel = currentFuel = 400;
        name = "airplane_" + airplaneNumber;
        ++airplaneNumber;

        maxCapacity = 500;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override List<Vector3> FindPath(City t_destination)
    {
        List<Vector3> path = new List<Vector3>();

        if (t_destination.cityType == CityType.airport)
        {
            Vector3 currPos = currentCity.transform.position;
            Vector3 destPos = t_destination.transform.position;

            Vector3 middlePoint = new Vector3( (currPos.x + destPos.x) / 2,
                                                ((currPos.y + destPos.y) / 2) + 5,
                                                (currPos.z + destPos.z) / 2);

            path.Add(currPos);
            path.Add(middlePoint);
            path.Add(destPos);
        }

        return path;
    }

    public override float CalculateFuelComsumption(List<Vector3> path)
    {
        return .0f;
    }
}
