using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextApp : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textContent;

    public string TextContent
    {
        get { return textContent.text; }
        set { textContent.text = value; }
    } 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
