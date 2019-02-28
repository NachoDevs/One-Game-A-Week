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

    //[HideInInspector]
    public GameObject selectedCity;

    [HideInInspector]
    public Dictionary<string, int> productList;

    // Start is called before the first frame update
    void Start()
    {
        InitializeProductList();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = money.ToString();

        if(upgradeMenu.gameObject.activeSelf)
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                upgradeMenu.gameObject.SetActive(false);
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
}
