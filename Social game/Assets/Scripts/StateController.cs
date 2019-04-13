using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    void StartOfAnimatiom()
    {
        GetComponent<CanvasRenderer>().SetAlpha(100);
    }

    void EndOfAnimation()
    {
        GetComponent<CanvasRenderer>().SetAlpha(0);
    }
}
