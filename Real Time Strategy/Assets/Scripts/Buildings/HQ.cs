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

        canBuild.Add(UnitType.builder);
        canBuild.Add(UnitType.soldier);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void LeftClick()
    {
        base.LeftClick();
    }
}
