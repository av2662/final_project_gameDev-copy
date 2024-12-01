using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;


    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;

    string masterVolume = "MasterVolume";
    string sfxVolume = "SFXVolume";
    string musicVolume = "MusicVolume";
    
   
    void Start(){
        masterVolumeSlider.value = PlayerPrefs.GetFloat(masterVolume, .1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(sfxVolume, .1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolume, .1f);
        SetMasterVolume();
        SetSFXVolume();
        SetMusicVolume();
    }
    

    public void SetMasterVolume(){
       SetVolume(masterVolume, masterVolumeSlider.value);
       PlayerPrefs.SetFloat(masterVolume, masterVolumeSlider.value);
    }
     public void SetSFXVolume(){
       SetVolume(sfxVolume, sfxVolumeSlider.value);
       PlayerPrefs.SetFloat(sfxVolume, sfxVolumeSlider.value);
    }
     public void SetMusicVolume(){
       SetVolume(musicVolume, musicVolumeSlider.value);
       PlayerPrefs.SetFloat(musicVolume, musicVolumeSlider.value);
    }
    void SetVolume(string groupName, float value){
        float adjustedVolume = Mathf.Log10(value) * 20;
        if(value == 0){
            adjustedVolume = -80;
        }
        audioMixer.SetFloat(groupName, adjustedVolume);
    }
}
