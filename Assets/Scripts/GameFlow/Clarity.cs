using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clarity : MonoBehaviour
{

    public TextMeshProUGUI writing;
    public Camera mainCamera;

    public string LastClickedWord;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var wordIndex = TMP_TextUtilities.FindIntersectingWord(writing, Input.mousePosition, mainCamera);

            if (wordIndex != -1)
            {
                LastClickedWord = writing.textInfo.wordInfo[wordIndex].GetWord();

                HyperInkWrapper.instance.GoToKnot(LastClickedWord);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
