using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Good
{
    public static int uniqueId = 0;

    public int id = 0;
    public string item;
    public int price;
    public int weight;
    public City owner;
    public City destination;

    public Good()
    {
        ++uniqueId;
        id = uniqueId;

        item = "";
        price = 0;
        weight = 0;
    }

    public Good(string t_item, int t_price, int t_weight, City t_owner, City t_destination)
    {
        ++uniqueId;
        id = uniqueId;

        item = t_item;
        price = t_price;
        weight = t_weight;
        owner = t_owner;
        destination = t_destination;
    }

}
