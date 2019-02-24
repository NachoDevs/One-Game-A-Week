using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    public void NewGame()
    {
        // We delete the save file (only one save file allowed)
        File.Delete(Application.persistentDataPath + "/saveFile.grw");

        // And then load the game with the fresh file
        LoadGame();
    }

    public void LoadGame()
    {
        // We just load the game, so it will take the previous save file
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        // Exit point
        Application.Quit();
    }
}
