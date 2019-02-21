using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plant : MonoBehaviour
{
    public float growingRate = .001f;
    public float growthState = .0f;

    public float waterLevel = 100f;
    public float foodLevel = 100f;

    public DateTime lastTimeConected;

    private float waterLossingRate = .01f;
    private float foodLossingRate = .001f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Grow", .0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void WaterPlant(float t_amount)
    {
        waterLevel+= t_amount;
    }

    public void FeedPlant(float t_amount)
    {
        foodLevel += t_amount;
    }

    public void SimulateSeconds(int t_seconds)
    {
        for(int i = 0; i < t_seconds; ++i)
        {
            Grow();
        }
    }

    private void Grow()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x,
                                                                        transform.localScale.y,
                                                                        transform.localScale.z + growingRate), Time.deltaTime);

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x + (growingRate * .1f),
                                                                                transform.localScale.y + (growingRate * .1f),
                                                                                transform.localScale.z), Time.deltaTime);

        waterLevel -= waterLossingRate;
        foodLevel -= foodLossingRate;

        lastTimeConected = DateTime.Now;
    }

    public double TimeDifference(DateTime t_past, DateTime t_now)
    {
        return t_now.Subtract(t_past).TotalSeconds;
    }
}
