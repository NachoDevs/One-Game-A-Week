using System;
using TMPro;
using UnityEngine;

public class GoodCard : MonoBehaviour, IComparable
{
    public Good goodData = new Good();

    private GameManager m_gm;

    // Start is called before the first frame update
    void Start()
    {
        m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = ToString() + " -> " + goodData.destination.name;
    }

    public void SelectGood()
    {
        if (m_gm.selectedCity != goodData.owner.gameObject)
        {
            if (m_gm.referencedVehicle.GetComponent<Truck>() != null)
            {
                m_gm.referencedVehicle.GetComponent<Truck>().collecting.Add(this);
            }
        }
        else
        {
            m_gm.referencedVehicle.GetComponent<Vehicle>().delivering.Add(this);
        }

        //GoodCard g = m_gm.selectedCity.GetComponent<City>().items.Find(x => x.Equals(gameObject));

        HideCard(true);

        m_gm.selectedCity.GetComponent<City>().items.Remove(this);

    }

    public override string ToString()
    {
        return goodData.item + " (" + goodData.price + ")";
    }

    public void HideCard(bool t_hide)
    {
        foreach (MonoBehaviour c in GetComponentsInChildren<MonoBehaviour>())
        {
            c.enabled = !t_hide;
        }

        foreach (MonoBehaviour c in GetComponents<MonoBehaviour>())
        {
            c.enabled = !t_hide;
        }

        GetComponent<GoodCard>().enabled = true;
    }

    public int CompareTo(object t_ob)
    {
        return (goodData.id == ((GoodCard)t_ob).goodData.id) ? 1 : 0;
    }
}
