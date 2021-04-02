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
    }

    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSON.text);
    }

    public void GoToKnot(string address)
    {
        story.ChoosePathString(address);
    }

    public void Choose(int choice)
    {
        if (story.canContinue)
        {
            story.ChooseChoiceIndex(choice);
        } else
        {
            Debug.LogError("Attempted to continue but could not (Ezra)");
        }
    }

    public string Continue()
    {
        return story.Continue();
    }
}
