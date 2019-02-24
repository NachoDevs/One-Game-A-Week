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
        LoadPlant();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(SaveCheckCanvas.activeSelf)
            {
                ToggleSaveCheck();
            }
            else
            {
                TogglMenu();
            }
        }

        switch(Input.inputString)
        {
            default:
                break;
        }

        waterSlider.value = plant.waterLevel;
        foodSlider.value = plant.foodLevel;

        moneyText.text = money.ToString();
    }

    public void Save()
    {
        SaveSystem.SaveGame(plant, this);
    }

    public void LoadPlant()
    {
        PlantData data = SaveSystem.LoadGame();

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
