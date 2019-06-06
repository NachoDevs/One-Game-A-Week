using UnityEngine;
using UnityEngine.UI;

//public enum BuildingType
//{
//    bakery,
//    farm,
//    lumber,
//    mine,
//    windmill
//}

public class Building : MonoBehaviour
{
    public bool isBuilded;

    public GameObject fullPopUp;

    protected int maxStorageCapacity;

    protected float currentAmount;
    protected float productionRate;

    protected Sprite resource;

    protected void Start()
    {
        fullPopUp.SetActive(false);
    }

    protected void Update()
    {
        currentAmount += productionRate * Time.deltaTime;
        if(currentAmount >= maxStorageCapacity)
        {
            currentAmount = maxStorageCapacity;

            MaxCapacityReached();
        }

        if(fullPopUp.activeSelf)
        {
            Vector3 pos = transform.position;
            pos.y += Mathf.Sin(Time.time * 5) * .25f + 1;
            fullPopUp.transform.position = pos;
        }
    }

    protected virtual void MaxCapacityReached()
    {
        fullPopUp.GetComponentsInChildren<Image>()[1].sprite = resource;

        fullPopUp.SetActive(true);
    }
}
