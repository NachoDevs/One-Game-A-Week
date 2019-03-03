using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleMiniPanel : MonoBehaviour
{
    public Vehicle referencedVehicle;

    public Slider fuelSlider;

    public Image colorImage;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        fuelSlider.maxValue = referencedVehicle.maxFuel;
        fuelSlider.value = referencedVehicle.currentFuel;
        colorImage.material.color = referencedVehicle.assignedColor;
    }
}
