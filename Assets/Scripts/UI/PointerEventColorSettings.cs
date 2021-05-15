using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
/// <summary>
/// The types of pointer events that PointerEventColorSettings can respond to.
/// </summary>
public enum PointerEventType
{
    Hover,
    ToggleSelect,
    Pressed
}

[System.Serializable]
/// <summary>
/// A setting which is given via the inspector, saying that the image should become the given color when the  given event type happens.
/// </summary>
public class PointerEventSetting
{
    public PointerEventType eventType;
    public Color color = Color.white;
}

/// <summary>
/// Component that updates a target image's color based on settings which respond to pointer events from this object.
/// </summary>
public class PointerEventColorSettings : MonoBehaviour,
    IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// The image whose color should be adjusted in response to pointer events on this object.
    /// Will try to get an image from this object if none is provided.
    /// </summary>
    public Image TargetImage;
    /// <summary>
    /// The default color the image should revert to if no event conditions are met.
    /// </summary>
    public Color DefaultColor = Color.white;
    /// <summary>
    /// An ordered list of settings saying what color the image should be for each pointer event.
    /// These settings are prioritized based on their ordering, with element 0 having highest priority.
    /// </summary>
    public List<PointerEventSetting> EventPriorities = new List<PointerEventSetting>();
    /// <summary>
    /// Settings for how the color easing should behave.
    /// </summary>
    public EaseSettings EaseSettings;

    /// <summary>
    /// The image to be manipulated.
    /// </summary>
    private Image image;
    /// <summary>
    /// Is this object being hovered over by the mouse?
    /// </summary>
    private bool hovered = false;
    /// <summary>
    /// Has this object been toggle-selected by the mouse?
    /// </summary>
    private bool selected = false;
    /// <summary>
    /// Is this object currently being pressed by the mouse?
    /// </summary>
    private bool pressed = false;

    /// <summary>
    /// Cache the target image and easing function for use later.
    /// </summary>
    private void Awake()
    {
        image = (TargetImage == null) ? GetComponent<Image>() : TargetImage;
    }

    /// <summary>
    /// Initially set the color to default, then try updating it based on settings.
    /// </summary>
    private void Start()
    {
        image.color = DefaultColor;
        UpdateColor();
    }

    public void SetSelected(bool val)
    {
        selected = val;
        UpdateColor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selected = !selected;
        UpdateColor();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        UpdateColor();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        UpdateColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        UpdateColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        UpdateColor();
    }

    /// <summary>
    /// Update the target image's color depending on which of the PointerEventSettings' conditions is met first.
    /// </summary>
    private void UpdateColor()
    {
        Color color = DefaultColor;
        foreach (PointerEventSetting setting in EventPriorities)
        {
            PointerEventType eventType = setting.eventType;
            if (eventType == PointerEventType.Hover && hovered ||
                eventType == PointerEventType.ToggleSelect && selected ||
                eventType == PointerEventType.Pressed && pressed)
            {
                color = setting.color;
                break;
            }
        }
        EaseColor(color);
    }

    /// <summary>
    /// Tweens the target image's color to the given value.
    /// </summary>
    /// <param name="targetColor">The color to set on the target image.</param>
    private void EaseColor(Color targetColor)
    {
        TargetImage.DOColor(targetColor, EaseSettings.Duration).SetEase(EaseSettings.EasingFunction);
    }
}