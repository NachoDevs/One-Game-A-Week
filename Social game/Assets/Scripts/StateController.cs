using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    void StartOfAnimatiom()
    {
        print("start");
        GetComponent<CanvasRenderer>().SetAlpha(100);
    }

    void EndOfAnimation()
    {
        print("end");
        GetComponent<CanvasRenderer>().SetAlpha(0);
    }
}
