using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Toggle)), RequireComponent(typeof(TextMeshProUGUI))]
public class ToggleText : MonoBehaviour
{
    [TextArea]
    public string onText, offText;

    public bool ToggleBool { set { thisText.text = value ? onText : offText;} }

    private Toggle thisToggle;
    private TextMeshProUGUI thisText;

    // Start is called before the first frame update
    void Awake()
    {
        
        thisText = GetComponent<TextMeshProUGUI>();
        thisToggle = GetComponent<Toggle>();

        ToggleBool = thisToggle.isOn;
    }
}
