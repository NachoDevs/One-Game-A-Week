using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public float clickPoints = 1;

    float pointsPerFrame = 0;

    GameManager m_gm;

    // Start is called before the first frame update
    void Start()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        m_gm.AddPoints(pointsPerFrame);
    }

}
