using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTarget : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform Target;

    private Vector3 offset;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint((Vector3)eventData.position + offset);
        point.z = 0;
        Target.position = point;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = Camera.main.WorldToScreenPoint(Target.position) - (Vector3)eventData.position;
        offset.z = 0;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
