using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public int money = 300000;

    [Header("UI")]
    public Canvas upgradeMenu;
    public Button harborButton;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI airportCostText;
    public TextMeshProUGUI harborCostText;
    public WarehousePanel warehousePanel;
    public GameObject firstWarehousePanel;
    public TextMeshProUGUI cityName;
    public Button trucksButton;
    public Button addTrucksButton;
    public Button specialVehicleButton;
    public Button addSpecialVehicleButton;
    public Image airplaneImage;
    public Image boatImage;
    public RectTransform vehiclesPanelTransform;
    public TextMeshProUGUI vehiclePanelText;
    public RectTransform vehicleInfoPanel;
    public Slider deliveringSlider;
    public TextMeshProUGUI deliverText;
    public Slider collectingSlider;
    public TextMeshProUGUI collectingText;
    public Button goButton;
    public TMP_Dropdown vehicleSelectionDropdown;

    [Header("Prefabs")]
    public GameObject truckPrefab;
    public GameObject airplanePrefab;
    public GameObject boatPrefab;
    public GameObject vehicleMiniPanelPrefab;
    public GameObject goodInfoPrefab;

    [Space]
    public Transform vehiclesParent;

    public GameObject selectedCity;

    [HideInInspector]
    public GameObject referencedVehicle;

    [HideInInspector]
    public Dictionary<string, int> productList;
    [HideInInspector]
    public List<City> allCities;

    // Start is called before the first frame update
    void Start()
    {
        InitializeProductList();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = money.ToString();

        //if(firstWarehousePanel.activeSelf)
        //{
        //    firstWarehousePanel.GetComponent<RectTransform>().localScale = new Vector3(1 + Mathf.Sin(Time.deltaTime) * 10,
        //                                                                                1 + Mathf.Sin(Time.deltaTime) * 10,
        //                                                                                1);
        //}

        if(upgradeMenu.gameObject.activeSelf)
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                upgradeMenu.gameObject.SetActive(false);
            }
        }

        if(vehicleInfoPanel.gameObject.activeSelf)
        {
            if (referencedVehicle.GetComponent<Vehicle>().delivering != null)
            {
                deliveringSlider.value = 0;
                foreach (GoodCard gc in referencedVehicle.GetComponent<Vehicle>().delivering)
                {
                    deliveringSlider.value += gc.goodData.weight;
                }
            }
            deliverText.text = deliveringSlider.value + "/" + deliveringSlider.maxValue;


            if(collectingSlider.gameObject.activeSelf)
            {
                if (referencedVehicle.GetComponent<Truck>().collecting != null)
                {
                    collectingSlider.value = 0;
                    foreach (GoodCard gc in referencedVehicle.GetComponent<Truck>().collecting)
                    {
                        collectingSlider.value += gc.goodData.weight;
                    }
                }

                collectingText.text = collectingSlider.value + "/" + collectingSlider.maxValue;
            }
        }

    }

    private void InitializeProductList()
    {
        productList = new Dictionary<string, int>();
        productList.Add("Wood", 2);
        productList.Add("Planks", 10);
        productList.Add("Coal", 5);
        productList.Add("Furniture", 15);
        productList.Add("Wheat", 2);
        productList.Add("Bread", 4);
    }

    public void UpgradeCity(bool t_isAirport)
    {
        if(t_isAirport)
        {
            selectedCity.GetComponent<City>().UpgradeCity(CityType.airport);
        }
        else
        {
            selectedCity.GetComponent<City>().UpgradeCity(CityType.harbor);
        }
        selectedCity = null;
        upgradeMenu.gameObject.SetActive(false);
    }

    public void EnterPlanningMode()
    {
        string vehicleName = vehicleSelectionDropdown.options[vehicleSelectionDropdown.value].text;

        referencedVehicle = GameObject.Find(vehicleName);

        vehicleInfoPanel.gameObject.SetActive(true);

        deliveringSlider.maxValue = referencedVehicle.GetComponent<Vehicle>().maxCapacity;

        int dCount = 0;
        if(referencedVehicle.GetComponent<Vehicle>().delivering != null)
        {
            foreach(GoodCard gc in referencedVehicle.GetComponent<Vehicle>().delivering)
            {
                dCount += gc.goodData.weight;
            }
        }
        deliveringSlider.value = dCount;


        // Only trucks from here
        if(referencedVehicle.GetComponent<Truck>() == null)
        {
            collectingSlider.gameObject.SetActive(false);

            return;
        }

        collectingSlider.gameObject.SetActive(true);

        collectingSlider.maxValue = referencedVehicle.GetComponent<Vehicle>().maxCapacity;

        int cCount = 0;
        if (referencedVehicle.GetComponent<Truck>().collecting != null)
        {
            foreach (GoodCard gc in referencedVehicle.GetComponent<Truck>().collecting)
            {
                cCount += gc.goodData.weight;
            }
        }
        collectingSlider.value = cCount;
    }

    public void ExecutePlant()
    {
        referencedVehicle.GetComponent<Vehicle>().delivering.Sort();
        referencedVehicle.GetComponent<Vehicle>().HideVehicle(false);

        foreach (GoodCard g in referencedVehicle.GetComponent<Vehicle>().delivering)
        {
            referencedVehicle.GetComponent<Vehicle>().MoveTo(g.goodData.destination);

            money += g.goodData.price;
        }
        referencedVehicle.GetComponent<Vehicle>().delivering.Clear();

        if(referencedVehicle.GetComponent<Truck>() != null)
        {
            referencedVehicle.GetComponent<Vehicle>().MoveTo(referencedVehicle.GetComponent<Truck>().wareHouse);
        }
    }
}
