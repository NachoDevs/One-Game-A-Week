using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : Vehicle
{
    [Header("Truck Specific")]
    public City wareHouse;

    public List<GoodCard> collecting = new List<GoodCard>();

    // Start is called before the first frame update
    void Start()
    {
        maxFuel = currentFuel = 250;
        name = "truck_" + truckNumber;
        ++truckNumber;

        maxCapacity = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Simple, not definitive pathfinding
    public override List<Vector3> FindPath(City t_destination)
    {
        City currCity = currentCity;

        List<City> visitedCities = new List<City>(); visitedCities.Add(currentCity);
        List<Vector3> path = new List<Vector3>();

        float maxDistance;
        while(t_destination != currCity)
        {
            maxDistance = int.MaxValue;
            List<City> neighbours = currCity.neighbours;
            foreach (City neighbour in neighbours)
            {
                float distanceToGoal = Vector3.Distance(neighbour.transform.position, t_destination.transform.position);
                if (distanceToGoal < maxDistance)
                {
                    maxDistance = distanceToGoal;
                    currCity = neighbour;
                }
            }
            path.Add(currCity.transform.position);
        }

        return path;
    }

    public override float CalculateFuelComsumption(List<Vector3> path)
    {
        return .0f;
    }

}
