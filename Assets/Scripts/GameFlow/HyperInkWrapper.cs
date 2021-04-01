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
        Debug.Log("STUB: going to address: " + address);

       story.ChoosePathString(address);
       story.Continue();
        //TODO unstub
    }

    public void Continue()
    {
        story.Continue();
    }
}
