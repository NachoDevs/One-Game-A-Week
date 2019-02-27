using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public enum CityType
{
    airport,
    city,
    factory,
    harbor,
    warehouse
}

public struct ItemPrice
{
    string  item;
    int     price;

    public ItemPrice(string t_item, int t_price)
    {
        item = t_item;
        price = t_price;
    }
}

public class City : MonoBehaviour
{
    public bool owned;

    public bool onCoast;

    public CityType cityType = CityType.city;

    public string produces;

    public List<ItemPrice> items;
    public List<ItemPrice> needs;

    private GameManager m_gm;

    [Header("Meshes")]
    public Mesh airportMesh;
    public Mesh cityMesh;
    public Mesh factoryMesh;
    public Mesh harborMesh;
    public Mesh warehouseMesh;

    [Header("UI")]
    public TextMeshProUGUI itemsText;
    public TextMeshProUGUI needsText;
    public TextMeshProUGUI producesText;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        produces = "Wood";

        items = new List<ItemPrice>();
        needs = new List<ItemPrice>();
        int randomInt = Random.Range(1, 3);
        for(int i = 0; i < randomInt; ++i)
        {
            needs.Add(new ItemPrice("Wood", 2));
        }

        randomInt = Random.Range(1, 3);
        for (int i = 0; i < randomInt; ++i)
        {
            needs.Add(new ItemPrice("Planks", 4));
        }

        itemsText.text = items.ToString();
        needsText.text = needs.ToString();
        producesText.text = produces;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
