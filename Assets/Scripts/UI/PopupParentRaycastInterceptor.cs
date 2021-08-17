using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PopupParentRaycastInterceptor : MonoBehaviour
{
    private Image thisImage;

    private void Awake()
    {
        thisImage = GetComponent<Image>();
        thisImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Image is on if any children are enabled
        thisImage.enabled = GetComponentsInChildren<Transform>().Length > 1;
    }
}
