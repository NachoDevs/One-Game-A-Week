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

    public Color assignedColor;

    public City currentCity;
    public City destination;

    [Header("Lists")]
    public List<Vector3> currentPath;
    public List<GoodCard> delivering = new List<GoodCard>();

    protected static int truckNumber;
    protected static int airplaneNumber;
    protected static int boatNumber;

    // Start is called before the first frame update
    void Start()
    {
        assignedColor = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));       
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            currentFuel -= fuelConsumptionModifier * Time.deltaTime;    // Moving
            transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, Time.deltaTime * 10);
            if(transform.position.Equals(destination.transform.position))
            {
                moving = false;
            }
        }
        else
        {
            currentFuel += fuelConsumptionModifier * Time.deltaTime;    // Refueling
        }
    }

    public void HideVehicle(bool t_hide)
    {
        GetComponentInChildren<MeshRenderer>().enabled = !t_hide;
        GetComponentInChildren<Collider>().enabled = !t_hide;

    }

    public void MoveTo(City t_destination)
    {
        //currentPath = FindPath(t_destination);

        moving = true;

        transform.LookAt(t_destination.transform.position);
        destination = t_destination;

        /*foreach(Vector3 position in currentPath)
        {
            transform.LookAt(position);
            transform.Translate(position);
        }*/
    }

    abstract public List<Vector3> FindPath(City t_destination);

    abstract public float CalculateFuelComsumption(List<Vector3> path);

}
