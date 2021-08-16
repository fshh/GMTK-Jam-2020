using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugVariables : MonoBehaviour
{
    public TextMeshProUGUI variablesText;

    // Update is called once per frame
    void Update()
    {
        variablesText.text = HyperInkWrapper.instance.GetVariables();
    }
}
