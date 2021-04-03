using System.Collections;
using System;
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
        try
        {
            story.ChoosePathString(address);
        }
        catch (Exception e)
        {
            Debug.Log("tried to go to address " + address + ", is this a typo? We couldn't find it. :(");
            throw;
        }
    }

    public void Choose(int choice)
    {
        story.ChooseChoiceIndex(choice);
    }

    public string Continue()
    {
        if (story.canContinue)
        {
            return story.ContinueMaximally();
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
