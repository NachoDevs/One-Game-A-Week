using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public Slider musicVolSlider;

    private AudioSource m_backgroundMusic;

    void Start()
    {
        m_backgroundMusic = GetComponent<AudioSource>();
    }

    void Update()
    {
        m_backgroundMusic.volume = musicVolSlider.value;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToggleOptionsMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }

    public void QuitGame()
    {
        QuitGame();
    }

}
