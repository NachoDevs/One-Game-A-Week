using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2D : MonoBehaviour
{
    PlatformEffector2D m_pe;

    void Start()
    {
        m_pe = GetComponentInChildren<PlatformEffector2D>();
    }

    internal IEnumerator InvertRotationalOffset()
    {
        m_pe.rotationalOffset = 180f;
        yield return new WaitForSeconds(.5f);
        m_pe.rotationalOffset = 0f;
    }
}
