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

    private bool waiting = false;
    public bool Waiting { get { return waiting; } private set { waiting = value; }}

    private void Awake()
    {
        instance = this;
        story = new Story(inkJSON.text);
        story.BindExternalFunction("wait", (float waitTime) => {
            wait(waitTime);
        });

    }

    //For using WaitUntil in coroutines
    public bool WaitDelegate()
    {
        return !Waiting;
    }

    //Please note - this doesn't actually do anything except set the variable, other scripts must respect the waiting boolean (or not)
    private void wait(float waitTime)
    {
        if(!Waiting)
        {
            StartCoroutine(waitHelper(waitTime));
        }
    }

    private IEnumerator waitHelper(float waitTime)
    {
        Waiting = true;
        yield return new WaitForSeconds(waitTime);
        Waiting = false;
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
        Debug.Log("called continue");
        if (story.canContinue)
        {
            return story.Continue(); //TODO might need to go back to ContinueMaximally
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

    public string[] getTags()
    {
        return story.currentTags.ToArray();
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
