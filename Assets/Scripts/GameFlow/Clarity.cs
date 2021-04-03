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

    private void Awake()
    {
        instance = this;
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
                HyperInkWrapper.instance.GoToKnot(writing.textInfo.wordInfo[wordIndex].GetWord());
                ContinueUntilChoice();
            }
        }
    }

    public void ContinueUntilChoice()
    {
        if (HyperInkWrapper.instance.CanContinue())
        {
            writing.text += HyperInkWrapper.instance.Continue();
        }

        choiceParent.Populate(HyperInkWrapper.instance.GetChoices());
    }

    public void Choose(int choice)
    {
        HyperInkWrapper.instance.Choose(choice);

        ContinueUntilChoice();
    }
}
