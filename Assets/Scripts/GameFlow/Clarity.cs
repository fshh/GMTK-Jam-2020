using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class Clarity : MonoBehaviour
{

    public TextMeshProUGUI writing;
    public Camera mainCamera;
    public static Clarity instance;
    public ChoiceParent choiceParent;

    private char HypertextSymbol = '^';

    private Dictionary<string, string> wordToKnotName;
    private AudioSource clarityVoice;
    private Queue<AudioClip> voiceLines;

    private static string AUDIO_FILE_PATH = "Wav Files/VO/"; 

    private void Awake()
    {
        instance = this;
        clarityVoice = GetComponent<AudioSource>();

        voiceLines = new Queue<AudioClip>();
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

        //dumb implementation of voice playing, probably want to include a pause later (Ezra)
        if (!clarityVoice.isPlaying && voiceLines.Count > 0)
        {
            clarityVoice.clip = voiceLines.Dequeue();
            clarityVoice.Play();
        }
    }

    public void ContinueUntilChoice()
    {
        if (HyperInkWrapper.instance.CanContinue())
        {
            wordToKnotName.Clear();

            writing.text += ParseForHypertext(HyperInkWrapper.instance.Continue());
            checkTags();
        }
        writing.text += "\n";

        choiceParent.Populate(HyperInkWrapper.instance.GetChoices());
    }

    public void checkTags()
    {
        string[] tags = HyperInkWrapper.instance.getTags();

        foreach(string tag in tags)
        {
            Debug.Log(tag);
            AudioClip toAdd = (AudioClip)Resources.Load(AUDIO_FILE_PATH + tag);
            if(toAdd == null)
            {
                Debug.LogError("Incorrect VO Tag, was unable to find " + tag + " in our resources folder :/ (Ezra)");
            } else
            {
                voiceLines.Enqueue(toAdd);
            }
        }
    }

    public void Choose(int choice)
    {
        HyperInkWrapper.instance.Choose(choice);

        ContinueUntilChoice();
    }

    //I'm doing this in an inefficient way first, may want to change to stringbuilder later (Ezra)
    public string ParseForHypertext(string input) 
    {
        int firstIndex = input.IndexOf(HypertextSymbol);

        if(firstIndex == -1) //then it wasn't found, return unparsed input
        {
            return input;
        }

        string beforeParse = input.Substring(0, firstIndex);
        string toParse = input.Substring(firstIndex);
        int firstSpace = toParse.IndexOf(' ');

        string afterParse;
        if(firstSpace == -1) //if no spaces, then it was the last word
        {
            afterParse = "";
        } else
        {
            afterParse = toParse.Substring(firstSpace);
            toParse = toParse.Substring(0, firstSpace);
        }


        string[] wordAndKnot = toParse.Split(HypertextSymbol); //1 is word, 2 is knot name (0 is an empty string)


        wordToKnotName.Add(wordAndKnot[1], wordAndKnot[2]);
        string parsed = "*" + wordAndKnot[1] + "*";

        return beforeParse + parsed + ParseForHypertext(afterParse);
    }
}
