using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugVariables : MonoBehaviour
{
    public TextMeshProUGUI variablesText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        variablesText.text = HyperInkWrapper.instance.GetVariables();
    }
}
