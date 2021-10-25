using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class ChangeImageWhilePressed : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Sprite Default;
    public Sprite Pressed;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = Default;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = Pressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = Default;
    }
}
