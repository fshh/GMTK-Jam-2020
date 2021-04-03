using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceParent : MonoBehaviour
{

    public GameObject choiceButton;
    public List<ChoiceButton> choices;

    private void Awake()
    {
        choices = new List<ChoiceButton>();
    }

    public void Populate(string[] choiceArray)
    {
        foreach(ChoiceButton choice in choices)
        {
            Destroy(choice.gameObject);
        }

        choices.Clear();

        int i = 0;
        foreach(string choiceString in choiceArray)
        {
            ChoiceButton currentChoice = Instantiate(choiceButton, transform).GetComponent<ChoiceButton>();
            choices.Add(currentChoice);
            currentChoice.id = i++;
            currentChoice.choiceString = choiceString;
            currentChoice.setText();
        }
    }
    
}
