using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class DebugMenuToggle : MonoBehaviour
{
    private static KeyCode TOGGLE_BUTTON_1 = KeyCode.LeftControl, TOGGLE_BUTTON_2 = KeyCode.LeftShift;

    public bool DebugOnByDefault;

    private bool childrenVisibleInteractible = false;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        //set to correct on/off
        childrenVisibleInteractible = DebugOnByDefault;
        SetVisibleInteractible(childrenVisibleInteractible);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(TOGGLE_BUTTON_1) && Input.GetKeyDown(TOGGLE_BUTTON_2))
        {
            childrenVisibleInteractible = !childrenVisibleInteractible;

            SetVisibleInteractible(childrenVisibleInteractible);
        }
    }

    private void SetVisibleInteractible(bool on)
    {
        canvasGroup.alpha = on ? 1f : 0f;
        canvasGroup.blocksRaycasts = on;
        canvasGroup.interactable = on;
    }
}