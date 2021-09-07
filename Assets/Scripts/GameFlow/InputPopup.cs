using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputPopup : MonoBehaviour
{
    public string inkVariableName, textToSubmit;

    private const string CONTINUE_CHOICE = "continue";

    public TextMeshProUGUI description;

    public void SetVariable()
    {
        HyperInkWrapper.instance.SetVariable(inkVariableName, textToSubmit);
        Clarity.Instance.ChooseByWord(CONTINUE_CHOICE);
        Destroy(gameObject);
    }

    public string Init(string descriptionText, string newInkVariableName)
    {
        description.text = descriptionText;
        inkVariableName = newInkVariableName;

        return CONTINUE_CHOICE;
    }

    public void updateSubmitText(string newValue)
    {
        textToSubmit = newValue;
    }
}