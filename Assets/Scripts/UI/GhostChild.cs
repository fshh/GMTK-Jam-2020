using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GhostChild : MonoBehaviour
{
    public Image image;
    public void SetPosition(RectTransform t)
    {
        Color temp = image.color;
        temp.a = 1;
        image.color = temp;
        GetComponent<RectTransform>().position = t.position;
        image.DOFade(0, 1);
    }
}
