using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clarity : MonoBehaviour
{

    public TextMeshProUGUI writing;
    public Camera mainCamera;
    public static Clarity instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var wordIndex = TMP_TextUtilities.FindIntersectingWord(writing, Input.mousePosition, mainCamera);

            if (wordIndex != -1)
            {
                HyperInkWrapper.instance.GoToKnot(writing.textInfo.wordInfo[wordIndex].GetWord());
            }
        }
    }

    public void Choose(int choice)
    {
        HyperInkWrapper.instance.Choose(choice);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
