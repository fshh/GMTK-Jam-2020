using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingSpeedSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI charactersPerSecondDisplay;

    private void Awake()
    {
        slider.minValue = 1f;
        slider.maxValue = Mathf.Max(slider.maxValue, ClarityText.CHARACTERS_PER_SECOND);
        slider.value = ClarityText.CHARACTERS_PER_SECOND;
    }

    public void OnSliderChanged()
    {
        string sliderValueString = slider.value.ToString();
        sliderValueString = sliderValueString.Substring(0, Mathf.Min(sliderValueString.Length, 4));
        charactersPerSecondDisplay.text = sliderValueString.Contains(".") ? sliderValueString : sliderValueString + ".0";
        ClarityText.CHARACTERS_PER_SECOND = slider.value;
    }
}
