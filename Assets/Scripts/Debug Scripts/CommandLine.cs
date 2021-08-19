using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.WebCam;
using System;
using TMPro;

public class CommandLine : MonoBehaviour
{
    public Dictionary<string, Action<string>> commands;
    public Dictionary<string, string> description; //first string is the key (also the command) the second is the description

    public static CommandLine instance;
    public TextMeshProUGUI helpText;
    public GameObject helpParent;
        
    //Add new actions here to extend functionality
    private Action<string> LoadLevel = null, GoToKnot = null;

    private Action<string> SetVariable = null;

    private void Awake()
    {
        instance = this;
        commands = new Dictionary<string, Action<string>>();
        description = new Dictionary<string, string>();

        //Add more to the dictionary here to add more commands
        //commands.Add("toggle", ToggleCamLayer);
        //descriptions.Add("toggle", "Toggles a camera layer. Syntax is \"toggle <Camera_Layer_Name>\""); //TODO make this work again
        commands.Add("load", LoadScene);
        description.Add("load", "Loads a scene, note that <Scene_Number> must be an integer. Syntax is \"load <Scene_Number>\"");
        commands.Add("go", GoToKnot);
        description.Add("go", "Moves to the specified knot. Syntax is go <Knot_Name>");
        commands.Add("set", SetVariable);
        description.Add("set", "Sets the specified variable to the specified value. Syntax is set <Variable_Name> <New_Value>");
    }

    public void ProcessCommand(string input)
    {
        string[] words = input.Split(' ');
        string command = words[0];
        string args = input.Replace(command, "").Trim();

        bool executedCommand = false;
        foreach (KeyValuePair<string, Action<string>> pair in commands)
        {
            if (command.ToUpper().Equals(pair.Key.ToUpper()))
            {
                if (pair.Value != null)
                {
                    pair.Value?.Invoke(args);
                }
                else
                {
                    Debug.Log("The command command \"" + command + "\" exists but had no action associated with it");
                }
                executedCommand = true;
            }
        }

        if (!executedCommand)
        {
            //fallthrough
            Debug.Log("Unable to find command \"" + command);
            Help();
        }
    }

    public void Help()
    {
        helpParent.SetActive(true);
        helpText.text = "";
        foreach (KeyValuePair<string, Action<string>> pair in commands)
        {
            string description1;
            if(!description.TryGetValue(pair.Key, out description1))
            {
                description1 = "Couldn't find description, pester the programmers to add one for the " + pair.Key + " command";
            }
            helpText.text += "<b>" + pair.Key + "</b>" + ": " + description1 + "\n\n";
        }
    }

    public void LoadScene(string sceneNum)
    {
        LoadLevel?.Invoke(sceneNum);
        int intSceneNum = 0;
        if (int.TryParse(sceneNum, out intSceneNum))
        {
            SceneManager.LoadScene(intSceneNum);
        }
        else
        {
            Debug.Log("Tried to load a scene, but it seems like you didn't enter a number. Try \"load <scene number>\"");
        }
    }
}