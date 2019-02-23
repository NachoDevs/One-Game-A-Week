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

        growthState += growingRate;

        if((int)growthState % 100 == 0 && growthState > 1)
        {
            Vector3 v = transform.position;
            v.y = UnityEngine.Random.Range(5, tip.transform.position.y - 5);
            GameObject go = Instantiate(fruit, v, new Quaternion(), fruitsParent);
            go.transform.Rotate(-90 + UnityEngine.Random.Range(-20.0f, 20.0f), 90, 0);
        }
    }

    public double TimeDifference(DateTime t_past, DateTime t_now)
    {
        return t_now.Subtract(t_past).TotalSeconds;
    }
}
