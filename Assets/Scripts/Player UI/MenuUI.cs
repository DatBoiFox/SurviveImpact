using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settings;
    public GameObject information;

    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider effectsSlider;

    public AudioMixer audioMixer;

    public void Back()
    {
        if(mainMenu != null)
            mainMenu.active = true;
        if(settings != null)
            settings.active = false;
        if(information != null)
            information.active = false;
    }

    public void StartGame()
    {
        Application.LoadLevel("Area_0");
    }
    
    public void Exit()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        mainMenu.active = false;
        settings.active = true;
        float value;
        audioMixer.GetFloat("music", out value);
        musicSlider.SetValueWithoutNotify(value);
        audioMixer.GetFloat("sounds", out value);
        effectsSlider.SetValueWithoutNotify(value);
    }

    public void OpenInformation()
    {
        mainMenu.active = false;
        information.active = true;
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("music", volume);
    }

    public void SetSoundsVolume(float volume)
    {
        audioMixer.SetFloat("sounds", volume);
    }

    public void ExitGameToMainMenu()
    {
        FindObjectOfType<GameManager>().ReturnToMainMenu();
    }

}
