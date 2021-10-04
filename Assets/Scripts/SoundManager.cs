using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private const String musicParam = "music-vol";
    private const String sfxParam = "sfx-vol";
    private const String ambienceParam = "ambience-vol";

    
    private static SoundManager instance;
    
    public GameObject MusicPlayer;
    public GameObject AmbiencePlayer;

    public AudioMixer mixer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        LoadPrefs();
    }

    private float sliderValueToVolumeValue(float sliderVolume)
    {
        return (sliderVolume - 0.5f) * 40f;
    }

    private float VolumeValueToSliderValue(float volume)
    {
        return (volume / 40f) + 0.5f;
    }
    
    private void setMixerVolume(String key, float value)
    {
        mixer.SetFloat(key, value);
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float sliderVolume)
    {
        setMixerVolume(musicParam, sliderValueToVolumeValue(sliderVolume));
    }

    public void SetSFXVolume(float sliderVolume)
    {
        setMixerVolume(sfxParam, sliderValueToVolumeValue(sliderVolume));
    }
    
    public void SetAmbienceVolume(float sliderVolume)
    {
        setMixerVolume(ambienceParam, sliderValueToVolumeValue(sliderVolume));
    }

    private void LoadPrefs()
    {
        SetMusicVolume(VolumeValueToSliderValue(PlayerPrefs.GetFloat(musicParam)));
        SetSFXVolume(VolumeValueToSliderValue(PlayerPrefs.GetFloat(sfxParam)));
        SetAmbienceVolume(VolumeValueToSliderValue(PlayerPrefs.GetFloat(ambienceParam)));
    }

    public float GetMusicVolume()
    {
        var param = PlayerPrefs.GetFloat(musicParam);
        var val =  VolumeValueToSliderValue(param);
        return val;
    }
    
    public float GetSFXVolume()
    {
        return VolumeValueToSliderValue(PlayerPrefs.GetFloat(sfxParam));
    }
    
    public float GetAmbienceVolume()
    {
        return VolumeValueToSliderValue(PlayerPrefs.GetFloat(ambienceParam));
    }
    
}
