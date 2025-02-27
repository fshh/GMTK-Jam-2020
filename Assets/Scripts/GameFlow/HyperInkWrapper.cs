﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Ink;
using System.IO;
using System.Linq;
using Application = UnityEngine.Application;

[System.Serializable]
public class TwoStrings : UnityEvent<string, string> { }

public class HyperInkWrapper : MonoBehaviour
{
    private Story story;

    public static HyperInkWrapper instance;

    //reaches out to other files to register the delete function from ink
    [HideInInspector]
    public TwoStrings Delete;

    private bool waiting = false;

    private static string INK_FILES_FOLDER_PATH = "Ink/MainStory";
    private string inkFileContents; //TODO not the best implementation, just a stop gap

    public bool Waiting { get { return waiting; } private set { waiting = value; }}

    private void Awake()
    {
        instance = this;
        
        string inkFolderPath = Application.streamingAssetsPath + "/" + INK_FILES_FOLDER_PATH;
        //Uses a linq expression to get a list of ink files, then selects the first of that list and gets its name
        string storyName = new DirectoryInfo(inkFolderPath).GetFiles().Where(fileInfo => fileInfo.Name.Equals("Story.ink", StringComparison.OrdinalIgnoreCase)).ToArray()[0].Name;
        
#pragma warning disable CS0612 // Technically this is deprecated but I think it still works in this version of unity. using this to suppress errors relating to it.
        StartCoroutine(LoadStreamingAsset(inkFolderPath + "/" + storyName));
#pragma warning restore CS0612 // 
        
        //Syntax taken from here: https://github.com/inkle/ink/blob/master/Documentation/RunningYourInk.md#using-the-compiler
        var compiler = new Ink.Compiler(inkFileContents, new Compiler.Options
        {
            countAllVisits = true,
            fileHandler = new UnityInkFileHandler(Path.Combine(Application.streamingAssetsPath, INK_FILES_FOLDER_PATH))
        });
        
        story = compiler.Compile();
    }

    private void Start()
    {
        CommandLine.instance.commands["set"] += SetVariableCaller;
    }

    private void OnDestroy()
    {
        CommandLine.instance.commands["set"] -= SetVariableCaller;
    }

    public string GetStateJson()
    {
        return story.state.ToJson();
    }

    public void SetState(string newState)
    {
        story.state.LoadJson(newState);
    }

    /// <summary>
    /// Args will be in the form <Variable_Name> <New_Value>
    /// </summary>
    /// <param name="args"></param>
    public void SetVariableCaller(string args)
    {
        string[] splits = args.Split(' ');
        if(splits.Length < 2)
        {
            Debug.Log("Need more args for set variable");
            return;
        }
        string variableName = splits[0];
        string newVal = splits[1];

        if(story.variablesState[variableName] == null)
        {
            Debug.Log("Cannot find variable with name: " + variableName);
            return;
        }

        int intVal;
        bool boolVal;
        if(int.TryParse(newVal, out intVal))
        {
            SetVariable(variableName, intVal);
        } else if (bool.TryParse(newVal, out boolVal))
        {
            SetVariable(variableName, boolVal);
        } else
        {
            SetVariable(variableName, newVal); //fallthrough is that it's a string
        }

        Debug.Log($"Set variable '{variableName}' to value '{newVal}'.");
    }

    //Note: code from this thread: https://stackoverflow.com/questions/47804594/read-and-write-file-on-streamingassetspath
    [System.Obsolete]
    IEnumerator LoadStreamingAsset(string fileName)
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
            result = File.ReadAllText(filePath);
        }

        inkFileContents = result;
    }

    public void ObserveVariable(string variableName, Story.VariableObserver observer)
    {
        story.ObserveVariable(variableName, observer);
    }

    public void SetVariable(string variableName, string newValue)
    {
        story.variablesState[variableName] = newValue;
    }

    public void SetVariable(string variableName, bool newValue)
    {
        story.variablesState[variableName] = newValue;
    }

    public void SetVariable(string variableName, int newValue)
    {
        story.variablesState[variableName] = newValue;
    }

    public IEnumerator FireDeleteAfterContinues(string startString, string endString)
    {
        //Wait for any processing of previous information
        yield return new WaitUntil(CanContinue);
        yield return new WaitForEndOfFrame();

        Delete.Invoke(startString, endString);

    }

    public void GoToKnot(string address)
    {
        if (story.CheckPathExists(address))
        {
            story.ChoosePathString(address);
        } else 
        {
            Debug.Log("tried to go to address \"" + address + "\", is this a typo? We couldn't find it. :(");
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
        return story.currentChoices.Select(c => c.text).ToArray();
    }

    public string GetVariables()
    {
        string toReturn = "";
        VariablesState state = story.variablesState;
        foreach(string variableName in state)
        {
            toReturn += variableName + ": " + state[variableName] + "\n";
        }
        return toReturn;
    }
}


//(Ezra) This is a disgusting workaround, apologies to god and to any other programmers who may be looking at this.
//Ideally we'd be able to include the ink compiler in the assembly for this file but we couldn't figure out how to do that.
public class UnityInkFileHandler : IFileHandler
{
    private readonly string rootDirectory;

    public UnityInkFileHandler(string rootDirectory)
    {
        this.rootDirectory = rootDirectory;
    }

    public string ResolveInkFilename(string includeName)
    {
        return Path.Combine(rootDirectory, includeName);
    }

    public string LoadInkFileContents(string fullFilename)
    {
        return File.ReadAllText(fullFilename);
    }
}