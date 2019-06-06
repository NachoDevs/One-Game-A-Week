using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : Building
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        maxStorageCapacity = 50;
        productionRate = 5;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void Clicked()
    {
        base.Clicked();
    }
}
