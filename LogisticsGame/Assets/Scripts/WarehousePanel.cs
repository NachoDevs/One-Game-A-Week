using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarehousePanel : MonoBehaviour
{
    public GameManager gm;

    public City referencedCity;

    public VehicleMiniPanel selectedVehicle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.selectedCity == null)
        {
            return;
        }

        referencedCity = gm.selectedCity.GetComponent<City>();

        if(referencedCity.owned)
        {
            gm.cityName.text = referencedCity.name;

            switch(referencedCity.cityType)
            {
                case CityType.airport:
                    gm.airplaneImage.gameObject.SetActive(true);
                    gm.boatImage.gameObject.SetActive(false);
                    gm.specialVehicleButton.interactable = true;
                    gm.addSpecialVehicleButton.interactable = true;
                    break;
                case CityType.harbor:
                    gm.airplaneImage.gameObject.SetActive(false);
                    gm.boatImage.gameObject.SetActive(true);
                    gm.specialVehicleButton.interactable = true;
                    gm.addSpecialVehicleButton.interactable = true;
                    break;
                default:
                    gm.specialVehicleButton.interactable = false;
                    gm.addSpecialVehicleButton.interactable = false;
                    break;
            }
        }

    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        //GetComponent<RectTransform>().anchoredPosition.Set(GetComponent<RectTransform>().anchoredPosition.x, 5);
        ShowTrucks();

        PopulateDropdown();
    }

    public void HidePanel()
    {
        //GetComponent<RectTransform>().anchoredPosition.Set(GetComponent<RectTransform>().anchoredPosition.x, -160);
        gameObject.SetActive(false);
    }

    private void PopulateDropdown()
    {
        if (referencedCity == null)
        {
            return;
        }

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (Truck truck in referencedCity.trucks)
        {
            options.Add(new TMP_Dropdown.OptionData(truck.name));
        }

        foreach (Vehicle sVehicle in referencedCity.specialVehicles)
        {
            options.Add(new TMP_Dropdown.OptionData(sVehicle.name));
        }

        gm.vehicleSelectionDropdown.ClearOptions();

        gm.vehicleSelectionDropdown.AddOptions(options);
    }

    public void AddTruck()
    {
        GameObject truck = Instantiate(gm.truckPrefab, referencedCity.transform.position, new Quaternion(), gm.vehiclesParent);
        truck.GetComponent<Vehicle>().HideVehicle(true);

        referencedCity.trucks.Add(truck.GetComponent<Truck>());

        truck.GetComponent<Truck>().wareHouse = referencedCity;

        gm.vehiclePanelText.text += truck.GetComponent<Vehicle>().name + " (" + truck.GetComponent<Vehicle>().currentFuel + ")\t";
        //AddVehicleCard(truck.GetComponent<Vehicle>(), referencedCity.trucks.Count, gm.vehiclesPanelTransform.rect.width / referencedCity.trucks.Count);

        gm.referencedVehicle = truck;

        ShowTrucks();

        gm.EnterPlanningMode();
    }

    public void ShowTrucks()
    {
        //foreach (Transform go in gm.vehiclesPanelTransform.transform)
        //{
        //    if(go == transform)
        //    {
        //        continue;
        //    }

        //    Destroy(go);
        //}

        gm.vehiclePanelText.text = "";

        if (referencedCity == null)
        {
            return;
        }

        if(referencedCity.trucks.Count == 0)
        {
            return;
        }

        //float xPos = gm.vehiclesPanelTransform.rect.width / referencedCity.trucks.Count;

        //int count = 0;
        foreach(Truck truck in referencedCity.trucks)
        {
            //++count;
            //AddVehicleCard(truck, count, xPos);
            gm.vehiclePanelText.text += truck.name + " (" + truck.currentFuel + ")\t";
        }

        PopulateDropdown();
    }

    public void AddSpecialVehicle()
    {
        GameObject toInstantiate;
        if(referencedCity.cityType == CityType.airport)
        {
            toInstantiate = gm.airplanePrefab;
        }
        else if( referencedCity.cityType == CityType.harbor)
        {
            toInstantiate = gm.boatPrefab;
        }
        else
        {
            return;
        }

        GameObject vehicle = Instantiate(toInstantiate, referencedCity.transform.position, new Quaternion(), gm.vehiclesParent);
        vehicle.GetComponent<Vehicle>().HideVehicle(true);

        referencedCity.specialVehicles.Add(vehicle.GetComponent<Vehicle>());

        gm.vehiclePanelText.text += vehicle.GetComponent<Vehicle>().name + " (" + vehicle.GetComponent<Vehicle>().currentFuel + ")\t";

        gm.referencedVehicle = vehicle;

        ShowSpecialVehicles();

        gm.EnterPlanningMode();
    }

    public void ShowSpecialVehicles()
    {
        //foreach (Transform go in gm.vehiclesPanelTransform.transform)
        //{
        //    if(go == transform)
        //    {
        //        continue;
        //    }

        //    Destroy(go);
        //}

        gm.vehiclePanelText.text = "";

        if (referencedCity == null)
        {
            return;
        }

        if (referencedCity.specialVehicles.Count == 0)
        {
            return;
        }

        //float xPos = gm.vehiclesPanelTransform.rect.width / referencedCity.trucks.Count;

        //int count = 0;
        foreach (Vehicle sVehicle in referencedCity.specialVehicles)
        {
            //++count;
            //AddVehicleCard(sVehicle, count, xPos);
            gm.vehiclePanelText.text += sVehicle.name + " (" + sVehicle.currentFuel + ")\t";
        }

        PopulateDropdown();
    }

    private void AddVehicleCard(Vehicle t_truck, int t_count, float t_xPos)
    {
        GameObject go = Instantiate(gm.vehicleMiniPanelPrefab, gm.vehiclesPanelTransform);
        go.GetComponent<VehicleMiniPanel>().referencedVehicle = t_truck;

        go.transform.position = new Vector3(t_xPos * t_count, gm.vehiclesPanelTransform.position.y, gm.vehiclesPanelTransform.position.z);
    }
}
