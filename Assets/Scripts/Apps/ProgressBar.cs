using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public CanvasGroup cancelButtonGroup;

    private const string CONTINUE_CHOICE = "continue";

    public string Init(float timeToComplete, bool isCancelable)
    {
        if (!isCancelable)
        {
            cancelButtonGroup.alpha = 0f;
            cancelButtonGroup.interactable = false;
            cancelButtonGroup.blocksRaycasts = false;
        }

        slider.DOValue(1f, timeToComplete);

        return CONTINUE_CHOICE;
    }
}
