using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextApp : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textContent = null;

    public string TextContent
    {
        get { return textContent.text; }
        set { textContent.text = value; }
    } 
}
