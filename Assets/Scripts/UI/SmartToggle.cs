using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SmartToggle : MonoBehaviour
{
    private Toggle toggleTarget = null;
    
    // Start is called before the first frame update
    void Start()
    {
        toggleTarget = GetComponent<Toggle>();
        if (toggleTarget.isOn)
        {
            toggleTarget.isOn = false;
            toggleTarget.isOn = true;
        }
    }
}
