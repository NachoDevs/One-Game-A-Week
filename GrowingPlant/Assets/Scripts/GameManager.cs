using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Plant plant;

    public GameObject UICanvas;
    public GameObject MenuCanvas;
    public GameObject SaveCheckCanvas;

    public Slider waterSlider;
    public Slider foodSlider;

    public TextMeshProUGUI moneyText;

    public int money;

    void Start()
    {
        // We load the game everytime we open the game scene
        LoadPlant();
    }

    void Update()
    {
        // Cheking for the "menu" key
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(SaveCheckCanvas.activeSelf)  // We are pressing "Quit" in the menu
            {
                ToggleSaveCheck();
            }
            else    // We are opening the main menu
            {
                TogglMenu();
            }
        }

        // Update resources UI
        waterSlider.value = plant.waterLevel;
        foodSlider.value = plant.foodLevel;

        // Update money UI
        moneyText.text = money.ToString();
    }

    public void Save()
    {
        SaveSystem.SaveGame(plant, this);
    }

    public void LoadPlant()
    {
        PlantData data = SaveSystem.LoadGame();

        // "There is no save file"
        if(data == null)
        {
            return;
        }

        plant.growthState = data.growthState;
        plant.waterLevel = data.waterLevel;
        plant.foodLevel = data.foodLevel;
        System.DateTime dt = new System.DateTime(data.dateTime[0], data.dateTime[1], data.dateTime[2], data.dateTime[3], data.dateTime[4], data.dateTime[5]);

        money = data.money;

        double secondsElapsed = System.DateTime.Now.Subtract(dt).TotalSeconds;

        plant.lastTimeConected = dt;

        plant.SimulateSeconds((int)secondsElapsed);
    }

    public void TogglMenu()
    {
        UICanvas.SetActive(!UICanvas.activeSelf);
        MenuCanvas.SetActive(!MenuCanvas.activeSelf);

        MenuCanvas.GetComponent<AudioSource>().Play();
    }

    public void ToggleSaveCheck()
    {
        SaveCheckCanvas.SetActive(!SaveCheckCanvas.activeSelf);
        MenuCanvas.SetActive(!MenuCanvas.activeSelf);

        MenuCanvas.GetComponent<AudioSource>().Play();
    }

    public void SaveAndQuit()
    {
        Save();
        Quit();
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

}
