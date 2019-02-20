using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Plant plant;

    void Start()
    {
        LoadPlant();
    }

    void Update()
    {
        switch(Input.inputString)
        {
            case "s":
                SaveSystem.SaveGame(plant);
                break;
            case "l":

                LoadPlant();

                break;
            default:

                break;
        }
    }

    private void LoadPlant()
    {
        PlantData data = SaveSystem.LoadGame();

        plant.growthState = data.growthState;
        System.DateTime dt = new System.DateTime(data.dateTime[0], data.dateTime[1], data.dateTime[2], data.dateTime[3], data.dateTime[4], data.dateTime[5]);

        double secondsElapsed = System.DateTime.Now.Subtract(dt).TotalSeconds;

        plant.lastTimeConected = dt;

        plant.SimulateSeconds((int)secondsElapsed);
    }

}
