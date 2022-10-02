using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaitTimeSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI timeDisplay;

    private void Awake()
    {
        slider.minValue = 0f;
        slider.maxValue = Mathf.Max(slider.maxValue, Clarity.WAIT_TIME);
        slider.value = Clarity.WAIT_TIME;
    }

    public void OnSliderChanged()
    {
        string sliderValueString = slider.value.ToString();
        sliderValueString = sliderValueString.Substring(0, Mathf.Min(sliderValueString.Length, 4));
        timeDisplay.text = sliderValueString.Contains(".") ? sliderValueString : sliderValueString + ".0";
        Clarity.WAIT_TIME = slider.value;
    }
}
