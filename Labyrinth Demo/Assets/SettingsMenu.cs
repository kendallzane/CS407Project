using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    // Use this for initialization
    public AudioMixer audioMixer;
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
            gc.GetComponent<BackgroundMusic>().SwitchLayers(0);
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
}
