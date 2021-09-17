using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ChoiceButton : MonoBehaviour
{

    public int id;
    public string choiceString;

    public TextMeshProUGUI displayText;

    private void OnEnable()
    {
        setText();
    }

    public void setText()
    {
        displayText.text = choiceString;
    }

    public void pressed()
    {
        Clarity.Instance.Choose(id);
    }
    
}
