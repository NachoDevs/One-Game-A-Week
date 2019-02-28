using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    public bool moving;

    public int maxCapacity;

    public float currentFuel;
    public float maxFuel;

    public float fuelConsumptionModifier;

    public City currentCity;

    [Header("Lists")]
    public List<Vector3> currentPath;
    public List<ItemPrice> cargo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            currentFuel -= fuelConsumptionModifier * Time.deltaTime;
        }
    }

    public void MoveTo(City t_destination)
    {
        currentPath = FindPath(t_destination);

        moving = true;

        foreach(Vector3 position in currentPath)
        {
            transform.LookAt(position);
            transform.Translate(position);
        }

        moving = false;
    }

    abstract public List<Vector3> FindPath(City t_destination);

    abstract public float CalculateFuelComsumption(List<Vector3> path);

}
