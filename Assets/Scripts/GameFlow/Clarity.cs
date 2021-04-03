using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clarity : MonoBehaviour
{

    public TextMeshProUGUI writing;
    public Camera mainCamera;
    public static Clarity instance;
    public ChoiceParent choiceParent;

    private Dictionary<string, string> wordToKnotName;

    private void Awake()
    {
        instance = this;
        wordToKnotName = new Dictionary<string, string>();
    }

    public void Start()
    {
        ContinueUntilChoice();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var wordIndex = TMP_TextUtilities.FindIntersectingWord(writing, Input.mousePosition, mainCamera);

            if (wordIndex != -1)
            {
                string word = writing.textInfo.wordInfo[wordIndex].GetWord();
                if (wordToKnotName.ContainsKey(word))
                {
                    HyperInkWrapper.instance.GoToKnot(wordToKnotName[word]);
                    ContinueUntilChoice();
                }
            }
        }
    }

    public void ContinueUntilChoice()
    {
        if (HyperInkWrapper.instance.CanContinue())
        {
            writing.text += HyperInkWrapper.instance.Continue(); //TODO fix weird behavior at the end
        }

        choiceParent.Populate(HyperInkWrapper.instance.GetChoices());
    }

    public void Choose(int choice)
    {
        HyperInkWrapper.instance.Choose(choice);

        ContinueUntilChoice();
    }
}
