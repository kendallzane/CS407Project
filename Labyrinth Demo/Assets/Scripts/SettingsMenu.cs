using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    // Use this for initialization
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetVolume(float volume)
    {
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        if(gc != null)
        {
            Debug.Log("Entered");
            Debug.Log(gc.GetComponent<BackgroundMusic>().standaloneBaseVolume);
            gc.GetComponent<BackgroundMusic>().standaloneBaseVolume = volume;
            gc.GetComponent<BackgroundMusic>().baseVolume = volume / 2;
            gc.GetComponent<BackgroundMusic>().layerVolume = volume;
            gc.GetComponent<BackgroundMusic>().sFXVolume = volume;
			gc.GetComponent<BackgroundMusic>().SwitchLayers(gc.GetComponent<BackgroundMusic>().currLayer);
        } else
        {
            audioMixer.SetFloat("Volume", volume);
        }
        //Debug.Log(volume);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
