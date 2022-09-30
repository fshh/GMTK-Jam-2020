using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputPopup : MonoBehaviour
{
    public string inkVariableName;
    public string textToSubmit;
    public bool shouldBeLowercase = false;

    private const string CONTINUE_CHOICE = "continue";

    public TextMeshProUGUI description;

    public void SetVariable()
    {
        string text = shouldBeLowercase ? textToSubmit.ToLower() : textToSubmit;
        HyperInkWrapper.instance.SetVariable(inkVariableName, text);
        Clarity.Instance.ChooseByWord(CONTINUE_CHOICE);
        Destroy(gameObject);
    }

    public string Init(string descriptionText, string newInkVariableName, bool shouldBeLowercase)
    {
        description.text = descriptionText;
        inkVariableName = newInkVariableName;
        this.shouldBeLowercase = shouldBeLowercase;

        return CONTINUE_CHOICE;
    }

    public void updateSubmitText(string newValue)
    {
        textToSubmit = newValue;
    }
}