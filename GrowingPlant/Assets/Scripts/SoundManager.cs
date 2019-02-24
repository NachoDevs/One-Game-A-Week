using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Image speakerUIImage;

    public Slider musicVolume;

    public Sprite speakerOn, speakerOff;

    public GameObject backgroundMusicManager;

    // Update is called once per frame
    void Update()
    {
        // Synchronize the volume controll with the volume
        backgroundMusicManager.GetComponent<AudioSource>().volume = musicVolume.value;
    }

    public void Mute()
    {
        // Toggle mute status
        backgroundMusicManager.GetComponent<AudioSource>().mute = !backgroundMusicManager.GetComponent<AudioSource>().mute;

        // Toggle between mute and unmute icons
        if(backgroundMusicManager.GetComponent<AudioSource>().mute)
        {
            speakerUIImage.sprite = speakerOff;
        }
        else
        {
            speakerUIImage.sprite = speakerOn;
        }
    }
}
