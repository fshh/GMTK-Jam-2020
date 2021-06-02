using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class TwoStrings : UnityEvent<string, string> { }

public class HyperInkWrapper : MonoBehaviour
{

    public TextAsset inkJSON;

    //TODO make this work with multiple include statements. use this link for more info https://github.com/inkle/ink/blob/master/Documentation/RunningYourInk.md#using-the-compiler

    private Story story;

    public static HyperInkWrapper instance;

    //reaches out to other files to register the delete function from ink
    [HideInInspector]
    public TwoStrings Delete;

    private bool waiting = false;

    private static string INK_FILES_FOLDER_PATH = "Ink";
    private static string INK_FILE_NAME = "Story.ink"; //TODO make this just be the first ink story in the folder
    private string inkFileContents; //TODO not the best implementation, just a stop gap

    public bool Waiting { get { return waiting; } private set { waiting = value; }}

    private void Awake()
    {
        instance = this;

        string inkFilePath = Application.streamingAssetsPath + "/" + INK_FILES_FOLDER_PATH + "/" + INK_FILE_NAME;

        StartCoroutine(loadStreamingAsset(inkFilePath));

        var compiler = new Ink.Compiler(inkFileContents);
        story = compiler.Compile();

        story.BindExternalFunction("wait", (float waitTime) => {
            wait(waitTime);
        });

        if (Delete == null)
        {
            Delete = new TwoStrings();
        }

        story.BindExternalFunction("delete", (string startString, string endString) => {
            StartCoroutine(FireDeleteAfterContinues(startString, endString));
        });

    }

    //Note: code from this thread: https://stackoverflow.com/questions/47804594/read-and-write-file-on-streamingassetspath
    IEnumerator loadStreamingAsset(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        string result;

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            //TODO currently if it runs this path, it will break because the time taken to return will cause other things to crash
            WWW www = new WWW(filePath);
            yield return www;
            result = www.text;
        }
        else
        {
            result = System.IO.File.ReadAllText(filePath);
        }

        inkFileContents = result;
    }

    public IEnumerator FireDeleteAfterContinues(string startString, string endString)
    {
        //Wait for any processing of previous information
        yield return new WaitUntil(CanContinue);
        yield return new WaitForEndOfFrame();

        Delete.Invoke(startString, endString);

    }

    ///For using WaitUntil in coroutines
    public bool WaitDelegate()
    {
        return !Waiting;
    }

    ///Please note - this doesn't actually do anything except set the variable, other scripts must respect the waiting boolean (or not)
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
