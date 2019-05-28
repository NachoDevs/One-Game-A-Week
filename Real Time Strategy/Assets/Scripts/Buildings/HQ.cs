using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HQ : BuildingTypeBase
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void LeftClick()
    {
        base.LeftClick();
        m_gm.CreateNewBuilding(BuildingType.collector, assignedBuilding.team, new Vector3(5,0,5));
    }
}
