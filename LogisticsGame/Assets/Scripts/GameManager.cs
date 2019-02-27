using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int Money;

    [HideInInspector]
    public Dictionary<string, int> productList;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeProductList()
    {
        productList.Add("Wood", 2);
        productList.Add("Planks", 10);
        productList.Add("Coal", 5);
        productList.Add("Furniture", 15);
        productList.Add("Wheat", 2);
        productList.Add("Bread", 4);
    }
}
