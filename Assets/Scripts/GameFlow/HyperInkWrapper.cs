using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;

public class HyperInkWrapper : MonoBehaviour
{

    public TextAsset inkJSON;
    public Story story;

    public static HyperInkWrapper instance;

    private void Awake()
    {
        instance = this;
        story = new Story(inkJSON.text);
    }

    public void GoToKnot(string address)
    {
        story.ChoosePathString(address);
    }

    public void Choose(int choice)
    {
        story.ChooseChoiceIndex(choice);
    }

    public string Continue()
    {
        if (story.canContinue)
        {
            return story.Continue();
        }
        else
        {
            Debug.Log("tried to continue but couldn't");
            return "Attempted to continue but could not";
        }
    }

    public bool CanContinue()
    {
        return story.canContinue;
    }

    public string[] GetChoices()
    {
        List<Choice> choices = story.currentChoices;
        string[] toReturn = new string[choices.Count];

        for(int i = 0; i < choices.Count; i++)
        {
            toReturn[i] = choices[i].text;
        }

        return toReturn;
    }
}
