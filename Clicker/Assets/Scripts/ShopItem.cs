using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum itemType
{
    improveClick,
    improveAuto
}

public class ShopItem : MonoBehaviour
{
    public int price;

    public itemType type;

    [Header("UI")]
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;

    static GameManager s_gm;

    int m_amount;

    Button m_itemButton;

    // Start is called before the first frame update
    void Start()
    {
        m_itemButton = GetComponent<Button>();

        if(s_gm == null)
        {
            s_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        switch(type)
        {
            case itemType.improveClick:
                itemName.text = "Click ++";
                price = 1;
                break;
            case itemType.improveAuto:
                itemName.text = "Auto ++";
                price = 2;
                break;
        }

        itemPrice.text = price.ToString();
        m_itemButton.onClick.AddListener(delegate { Clicked(); });
    }

    public void Clicked()
    {
        if(s_gm.money < price)
        {
            return;
        }

        ++m_amount;

        s_gm.money -= price;
        price += m_amount;
        itemPrice.text = price.ToString();

        switch (type)
        {
            case itemType.improveClick:
                s_gm.IncreaseCick(1);
                break;
            case itemType.improveAuto:
                s_gm.IncreaseAuto(.01f);
                break;
        }
    }
}
