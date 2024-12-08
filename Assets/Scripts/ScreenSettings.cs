using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScreenSettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullScreenToggle;
    Resolution[] resolutions;

    void Start(){
        resolutions = Screen.resolutions;
        Resolution currentResolution = Screen.currentResolution;
        int currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", resolutions.Length - 1);
        for(int i = 0; i < resolutions.Length; i++){
            string resoltionstring = resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString();
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resoltionstring));
        }
        currentResolutionIndex = Math.Min(currentResolutionIndex, resolutions.Length - 1);
        resolutionDropdown.value = currentResolutionIndex;
        SetResolution();

       if (fullScreenToggle != null)
        {
            fullScreenToggle.isOn = Screen.fullScreen;
            fullScreenToggle.onValueChanged.AddListener(OnFullScreenToggleChanged);
        }

       
    }
    public void SetResolution(){
        int rezIndex = resolutionDropdown.value;
        Screen.SetResolution(resolutions[rezIndex].width, resolutions[rezIndex].height, true);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
    }

    void OnFullScreenToggleChanged(bool isOn)
    {
        Screen.fullScreen = isOn;
        
    }
    
}
