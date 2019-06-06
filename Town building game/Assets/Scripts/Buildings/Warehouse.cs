using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : Building
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        hasPopUp = false;
        maxStorageCapacity = 100;
        productionRate = 5;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void Clicked()
    {
    }
}
