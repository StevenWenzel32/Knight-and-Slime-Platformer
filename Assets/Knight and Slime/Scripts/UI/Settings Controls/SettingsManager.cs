using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // only have one instance ever, can get else where but can only set here -- singleton
    public static SettingsManager instance {get; private set;}

    // array of resolutions avaliable to the computer
    Resolution[] resolutions;
    // UI stuff
    public TMP_Dropdown resolutionDropdown;

    // UI for the volume control
    public Slider volumeSlider;
    public TextMeshProUGUI volumeText;

    // runs on start -- loads the saves or uses default for all the settings
    private void Start()
    {
        // singleton stuff
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
        
        InitializeResolutions();
        LoadQuality();
        LoadVolume();
        LoadFullscreen();
    }

    private void LoadQuality(){
        int savedQuality = PlayerPrefs.GetInt("quality", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(savedQuality);
    }

    private void LoadFullscreen(){
        bool isFullscreen = PlayerPrefs.GetInt("fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        Screen.fullScreen = isFullscreen;
    }

    private void LoadVolume(){
        float savedVolume = PlayerPrefs.GetFloat("volume", 0.5f);
        volumeSlider.value = savedVolume;
        SoundManager.instance.SetVolume(savedVolume);
        UpdateVolumeText(savedVolume);
    }

    // fill the options for the resolution drop down and pick the saved setting or the system default
    private void InitializeResolutions(){
        // get avaliable resolutions
        resolutions = Screen.resolutions;
        // clear the dropdown
        resolutionDropdown.ClearOptions();
        // index of the currently used resolution
        int currentResolutionIndex = 0;
        // create a list to hold the strings of options
        List<string> options = new List<string>();

        // create strings and put them into the list
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            // once the current resolution in the list matches the one being used save that index
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // put in all the resolutions in the array into the dropdown
        resolutionDropdown.AddOptions(options);

        // load the saved setting or use the current system resolution
        int savedResolutionIndex = PlayerPrefs.GetInt("resolution", currentResolutionIndex);
        // set the current choice to the current resolution
        resolutionDropdown.value = savedResolutionIndex;
        // update the dropdown
        resolutionDropdown.RefreshShownValue();
    }

    public void OnVolumeSliderChange(float volume){
        // call the soundManager to set the volume
        SoundManager.instance.SetVolume(volume);
        UpdateVolumeText(volume);

        // save the setting
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }

    public void UpdateVolumeText(float volume){
        if (volumeText != null){
            volumeText.text = Math.Round(volume).ToString();
        }
    }

    // changes the current game quailty to match the choice in settings
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
        PlayerPrefs.Save();
    }

    // turns fullscreen on and off using the fullscreen button in the settings
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        // might change to a bool
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    // changes the resolution to match the one chosen in the dropdown
    public void SetResolution(int index)
    {
        // make the change based on picked option
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);

        // save the change
        PlayerPrefs.SetInt("resolution", index);
        PlayerPrefs.Save();
    }
}
