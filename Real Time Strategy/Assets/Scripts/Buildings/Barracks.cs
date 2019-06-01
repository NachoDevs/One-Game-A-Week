using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : BuildingTypeBase
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

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
