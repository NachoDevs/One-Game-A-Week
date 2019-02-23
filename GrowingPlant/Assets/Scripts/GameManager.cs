using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Plant plant;

    public Slider waterSlider;
    public Slider foodSlider;

    public TextMeshProUGUI moneyText;

    public int money;

    void Start()
    {
        LoadPlant();
    }

    void Update()
    {
        switch(Input.inputString)
        {
            case "o":
                SaveSystem.SaveGame(plant, this);
                break;
            case "l":
                LoadPlant();
                break;
            default:

                break;
        }

        waterSlider.value = plant.waterLevel;
        foodSlider.value = plant.foodLevel;

        moneyText.text = money.ToString();
    }

    private void LoadPlant()
    {
        PlantData data = SaveSystem.LoadGame();

        plant.growthState = data.growthState;
        plant.waterLevel = data.waterLevel;
        plant.foodLevel = data.foodLevel;
        System.DateTime dt = new System.DateTime(data.dateTime[0], data.dateTime[1], data.dateTime[2], data.dateTime[3], data.dateTime[4], data.dateTime[5]);

        money = data.money;

        double secondsElapsed = System.DateTime.Now.Subtract(dt).TotalSeconds;

        plant.lastTimeConected = dt;

        plant.SimulateSeconds((int)secondsElapsed);
    }

}
