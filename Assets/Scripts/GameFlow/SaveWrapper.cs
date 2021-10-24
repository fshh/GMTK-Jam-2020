using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveManagement;

public class SaveWrapper : Singleton<SaveWrapper>
{
    public static string INK_STATE = "InkState";
    public static string CLARITY_TEXT = "ClarityText";
    public static string NEXT_CHOICE = "NextChoice";

    public void SaveStory(int nextChoice)
    {
        SaveManager.SetData(CLARITY_TEXT, Clarity.Instance.clarityText.Text);
        SaveManager.SetData(INK_STATE, HyperInkWrapper.instance.GetStateJson());
        SaveManager.SetData(NEXT_CHOICE, nextChoice);
    }

    /// <summary>
    /// Loads the story, with side effects
    /// </summary>
    /// <returns>The next choice ID, returns -1 if no choice yet selected</returns>
    public int LoadStory()
    {
        if (SaveManager.GetData(INK_STATE) == null) //If we haven't saved anything yet, don't do anything
        {
            return -1; 
        }
        else
        {
            HyperInkWrapper.instance.SetState((string) SaveManager.GetData(INK_STATE));
            Clarity.Instance.clarityText.Text = (string) SaveManager.GetData(CLARITY_TEXT);
            return (int)SaveManager.GetData(NEXT_CHOICE);
        }
    }

    public void ClearStory()
    {
        SaveManager.WipeFile();
    }
}
