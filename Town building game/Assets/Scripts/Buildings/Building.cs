using System;
using UnityEngine;
using UnityEngine.UI;

public enum BuildingType
{
    bakery,
    deliveryHouse,
    farm,
    lumber,
    mine,
    warehouse,
    windmill
}

public class Building : MonoBehaviour
{
    public bool isBuilded;

    public int woodCost;
    public int stoneCost;
    public int wheatCost;
    public int breadCost;

    public int maxStorageCapacity;

    public TileType whereToBuild;

    public GameObject fullPopUp;

    public Sprite resource;

    protected bool hasPopUp = true;

    protected float currentAmount;
    protected float productionRate;

    protected static GameManager m_gm;

    protected void Start()
    {
        fullPopUp.SetActive(false);

        CheckGM();
    }

    protected void Update()
    {
        currentAmount += productionRate * Time.deltaTime;
        if(currentAmount >= maxStorageCapacity)
        {
            currentAmount = maxStorageCapacity;

            MaxCapacityReached();
        }

        if(hasPopUp)
        {
            if(fullPopUp.activeSelf)
            {
                Vector3 pos = transform.position;
                pos.y += Mathf.Sin(Time.time * 5) * .25f + 1;
                fullPopUp.transform.position = pos;
            }
        }
    }

    public bool CanAfford()
    {
        CheckGM();

        return m_gm.resourceCount[ResourceType.wood] >= woodCost
            && m_gm.resourceCount[ResourceType.stone] >= stoneCost
            && m_gm.resourceCount[ResourceType.wheat] >= wheatCost
            && m_gm.resourceCount[ResourceType.bread] >= breadCost;
    }

    public void Build()
    {
        m_gm.UpdateResource(ResourceType.wood, -woodCost);
        m_gm.UpdateResource(ResourceType.stone, -stoneCost);
        m_gm.UpdateResource(ResourceType.wheat, -wheatCost);
        m_gm.UpdateResource(ResourceType.bread, -breadCost);

        isBuilded = true;
    }

    public virtual void Clicked()
    {
        fullPopUp.SetActive(false);
        ResourceType rt;
        Enum.TryParse(resource.name, out rt);
        m_gm.UpdateResource(rt, (int)currentAmount);
        currentAmount = 0;
    }

    protected virtual void MaxCapacityReached()
    {
        if (hasPopUp)
        {
            fullPopUp.GetComponentsInChildren<Image>()[1].sprite = resource;

            fullPopUp.SetActive(true);
        }
    }

    protected void CheckGM()
    {
        if (m_gm == null)
        {
            m_gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    }

    internal bool CanBuildHere(GameObject m_tile)
    {
        try
        {
            if(m_tile.GetComponent<WorldTile>() == null)
            {
                return false;
            }
        }
        catch (Exception e) { return false; }

        return m_tile.GetComponent<WorldTile>().tileType == whereToBuild;
    }
}
