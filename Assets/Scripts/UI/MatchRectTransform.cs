using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRectTransform : MonoBehaviour
{
    public RectTransform reference;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // System.Reflection.FieldInfo[] fields = reference.GetType().GetFields();
        // foreach (System.Reflection.FieldInfo field in fields)
        // {
        //     field.SetValue(rectTransform, field.GetValue(reference));
        // }

        // rectTransform.anchorMax = reference.anchorMax;
        // rectTransform.anchorMin = reference.anchorMin;
        // rectTransform.pivot = reference.pivot;
        // rectTransform.offsetMax = reference.offsetMax;
        // rectTransform.offsetMin = reference.offsetMin;
        // rectTransform.anchoredPosition3D = reference.anchoredPosition3D;

        rectTransform.pivot = reference.pivot;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, reference.rect.width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, reference.rect.height);
        rectTransform.position = reference.position;
    }
}
