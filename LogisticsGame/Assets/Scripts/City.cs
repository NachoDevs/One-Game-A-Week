using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum CityType
{
    airport,
    city,
    factory,
    harbor,
    warehouse
}

public class City : MonoBehaviour
{
    public bool owned;
    public bool clicked;
    public bool onCoast;

    public int buyingCost = 250000;
    public int upgradeCost = 350000;

    public CityType cityType = CityType.city;

    public Material notOwnedMaterial;
    public Material hoverNotOwnedMaterial;
    public Material ownedMaterial;
    public Material hoverOwnedMaterial;

    public List<Truck> trucks;
    public List<Vehicle> specialVehicles;

    public List<GoodCard> items;

    private GameManager m_gm;

    private RaycastHit hit;

    [Header("UI")]
    public Canvas popUpInfo;
    public RectTransform goodsScrollView;
    public Button buyButton;

    [Header("Meshes")]
    public Mesh airportMesh;
    public Mesh cityMesh;
    public Mesh factoryMesh;
    public Mesh harborMesh;
    public Mesh warehouseMesh;

    [Header("Neighbours")]
    public List<City> neighbours;

    // Start is called before the first frame update
    void Start()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        m_gm.airportCostText.text = upgradeCost.ToString();
        m_gm.harborCostText.text = upgradeCost.ToString();

        trucks = new List<Truck>();
        specialVehicles = new List<Vehicle>();

        items = new List<GoodCard>();

        int randomInt = Random.Range(1, 5);
        for (int i = 0; i < randomInt; ++i)
        {
            int randomInt2 = Random.Range(0, m_gm.allCities.Count);
            Good g = new Good("Wood", 2, 20, this, m_gm.allCities[randomInt2]);

            GameObject good = Instantiate(m_gm.goodInfoPrefab, goodsScrollView.transform);

            good.GetComponent<GoodCard>().goodData.item = g.item;
            good.GetComponent<GoodCard>().goodData.price = g.price;
            good.GetComponent<GoodCard>().goodData.weight = g.weight;
            good.GetComponent<GoodCard>().goodData.owner = g.owner;
            good.GetComponent<GoodCard>().goodData.destination = g.destination;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // PopUp show system
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (!clicked)
            {
                // Mouse feedback while hovering a city
                if(hit.collider == GetComponentInChildren<MeshCollider>())
                {
                    if (owned)
                    {
                        GetComponentInChildren<Renderer>().material = hoverOwnedMaterial;
                    }
                    else
                    {
                        GetComponentInChildren<Renderer>().material = hoverNotOwnedMaterial;
                    }
                }
                else
                {
                    if (owned)
                    {
                        if (GetComponentInChildren<Renderer>().material != ownedMaterial)
                        {
                            GetComponentInChildren<Renderer>().material = ownedMaterial;
                        }
                    }
                    else
                    {
                        if (GetComponentInChildren<Renderer>().material != notOwnedMaterial)
                        {
                            GetComponentInChildren<Renderer>().material = notOwnedMaterial;
                        }
                    }
                }

                popUpInfo.gameObject.SetActive(hit.collider == GetComponentInChildren<MeshCollider>());

                if (Input.GetMouseButtonUp(0))
                {
                    clicked = true;
                    if(hit.transform.parent.GetComponent<City>() != null)
                    {
                        m_gm.selectedCity = hit.transform.parent.gameObject;
                        if(hit.transform.parent.GetComponent<City>().owned)
                        {
                            m_gm.warehousePanel.ShowPanel();
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    clicked = false;
                    //if(hit.transform.parent.GetComponent<WarehousePanel>() == null)
                    //{
                    //    m_gm.warehousePanel.HidePanel();
                    //}
                }
            }
        }

        if (buyButton.IsActive())
        {
            if(!owned)
            {
                buyButton.interactable = m_gm.money >= buyingCost;
            }
            else
            {
                buyButton.interactable = m_gm.money >= upgradeCost;
            }
        }

    }

    public void ButtonFunctionality()
    {
        if(!owned)
        {
            BuyCity();

            m_gm.firstWarehousePanel.SetActive(false);
        }
        else
        {
            ShowUpgradeMenu();
        }
    }

    private void ShowUpgradeMenu()
    {
        m_gm.upgradeMenu.gameObject.SetActive(true);

        if(!onCoast)
        {
            m_gm.harborButton.interactable = false;
        }
    }

    // BuyCity and UpgradeCity could probably be merged in just one function
    private void BuyCity()
    {
        owned = true;
        buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade!";

        GetComponentInChildren<Renderer>().material = ownedMaterial;

        GetComponentInChildren<MeshFilter>().mesh = warehouseMesh;
        GetComponentInChildren<MeshCollider>().sharedMesh = warehouseMesh;

        if(!onCoast)
        {
            GetComponentInChildren<MeshFilter>().transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
        }

        cityType = CityType.warehouse;

        m_gm.money -= buyingCost;
    }

    public void UpgradeCity(CityType t_ct)
    {
        buyButton.gameObject.SetActive(false);

        switch(t_ct)
        {
            case CityType.airport:
                GetComponentInChildren<MeshFilter>().mesh = airportMesh;
                GetComponentInChildren<MeshCollider>().sharedMesh = airportMesh;
                GetComponentInChildren<MeshFilter>().transform.eulerAngles.Set(0, 0, Random.Range(0, 360));
                cityType = CityType.airport;
                break;
            case CityType.harbor:
                GetComponentInChildren<MeshFilter>().mesh = harborMesh;
                GetComponentInChildren<MeshCollider>().sharedMesh = harborMesh;
                cityType = CityType.harbor;
                break;
            default:
                break;
        }

        m_gm.money -= upgradeCost;
    }
}
