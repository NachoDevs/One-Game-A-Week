using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plant : MonoBehaviour
{
    public GameObject fruit;

    public GameObject tip;

    public Transform fruitsParent;

    public float growingRate = .001f;
    public float growthState = .0f;

    public float waterLevel = 100f;
    public float foodLevel = 100f;

    public DateTime lastTimeConected;

    private float waterLossingRate = 1;
    private float foodLossingRate = 1;

    // Start is called before the first frame update
    void Start()
    {
        // This is the method which allows the plant to grow, which will be called every second
        InvokeRepeating("Grow", .0f, 1.0f);
    }

    public void WaterPlant(float t_amount)
    {
        waterLevel+= t_amount;
    }

    public void FeedPlant(float t_amount)
    {
        foodLevel += t_amount;
    }

    // This will simulate the growth of the plant when we are not connected
    public void SimulateSeconds(int t_seconds)
    {
        for(int i = 0; i < t_seconds; ++i)
        {
            Grow();
        }
    }

    private void Grow()
    {
        // Increment the height
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x,
                                                                        transform.localScale.y,
                                                                        transform.localScale.z + growingRate), Time.deltaTime);

        // Increment the width
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x + (growingRate * .1f),
                                                                                transform.localScale.y + (growingRate * .1f),
                                                                                transform.localScale.z), Time.deltaTime);

        // Resource loss
        waterLevel -= waterLossingRate;
        foodLevel -= foodLossingRate;

        // Update the timer (for simulation purposes)
        lastTimeConected = DateTime.Now;

        growthState += growingRate;

        // Fruit spawner
        if((int)growthState % 100 == 0 && growthState > 1)
        {
            Vector3 v = transform.position;
            v.y = UnityEngine.Random.Range(5, tip.transform.position.y - 5);
            GameObject go = Instantiate(fruit, v, new Quaternion(), fruitsParent);
            go.transform.Rotate(-90 + UnityEngine.Random.Range(-20.0f, 20.0f), 90, 0);
        }
    }
}
