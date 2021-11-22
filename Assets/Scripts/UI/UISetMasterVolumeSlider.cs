using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UISetMasterVolumeSlider : MonoBehaviour
{
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _slider;

    void Start()
    {
        _slider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
    }

    public void SetLevel()
    {
        float sliderValue = _slider.value;
        _mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }
}
