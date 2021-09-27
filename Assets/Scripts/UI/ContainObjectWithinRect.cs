using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ContainObjectWithinRect : MonoBehaviour
{
    public RectTransform ObjectToContain;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        Rect rect = rectTransform.rect;

        float leftSide = rectTransform.anchoredPosition.x - rect.width / 2;
        float rightSide = rectTransform.anchoredPosition.x + rect.width / 2;
        float topSide = rectTransform.anchoredPosition.y + rect.height / 2;
        float bottomSide = rectTransform.anchoredPosition.y - rect.height / 2;

        ObjectToContain.anchoredPosition = new Vector2(
            Mathf.Clamp(ObjectToContain.anchoredPosition.x, leftSide, rightSide), 
            Mathf.Clamp(ObjectToContain.anchoredPosition.y, bottomSide, topSide));
    }
}
