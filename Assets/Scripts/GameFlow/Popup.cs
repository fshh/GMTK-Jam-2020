﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    public string inkVariableName;

    public TextMeshProUGUI description, collapsedInfo, buttonOne, buttonTwo;

    private string[] buttonNames;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseWord(int buttonNumber)
    {
        Clarity.Instance.ChooseByWord(buttonNames[buttonNumber]);
        Destroy(gameObject);
    }

    public void Init(string descriptionText, string extraDescription, string buttonOneText, string buttonTwoText)
    {
        buttonNames = new string[2];

        description.text = descriptionText;
        collapsedInfo.text = extraDescription;

        buttonOne.text = buttonOneText;
        buttonNames[0] = buttonOneText;

        buttonTwo.text = buttonTwoText;
        buttonNames[1] = buttonTwoText;
    }
}
