using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerController : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer Mixer;

    [Header("Sliders")]
    public Slider MasterSlider;
    public Slider VoiceSlider;
    public Slider SFXSlider;

    private void Start()
    {
        LoadVolumePreferences();
    }

    // 1 is max volume, 0.0001 is min
    private void LoadVolumePreferences()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1);
        float voice = PlayerPrefs.GetFloat("VoiceVolume", 1);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1);

        MasterSlider.value = master;
        VoiceSlider.value = voice;
        SFXSlider.value = sfx;

        SetMasterVolume(master);
        SetVoiceVolume(voice);
        SetSFXVolume(sfx);
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume("MasterVolume", volume);
    }

    public void SetVoiceVolume(float volume)
    {
        SetVolume("VoiceVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume("SFXVolume", volume);
    }

    private void SetVolume(string varName, float volume)
    {
        PlayerPrefs.SetFloat(varName, volume);
        PlayerPrefs.Save();

        // Adjust volume on logarithmic scale for smooth adjustments
        Mixer.SetFloat(varName, Mathf.Log10(volume) * 20f);
    }
}
