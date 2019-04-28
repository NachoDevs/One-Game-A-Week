using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    public float startTimeBetweenSpawns;

    public GameObject echo;

    float m_timeBetweenSpawns;

    void Update()
    {
        if(m_timeBetweenSpawns <= 0)
        {
            GameObject go = Instantiate(echo, transform.position, transform.rotation);
            Destroy(go, .5f);
            m_timeBetweenSpawns = startTimeBetweenSpawns;
        }
        else
        {
            m_timeBetweenSpawns -= Time.deltaTime;
        }
    }

}
