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

    private Dictionary<string, int> wordToChoiceIndex;
    private AudioSource clarityVoice;
    private Queue<AudioClip> voiceLines;

    private List<string> currChoices;

    private static string AUDIO_FILE_PATH = "Wav Files/VO/"; 

    private void Awake()
    {
        instance = this;
        clarityVoice = GetComponent<AudioSource>();

        currChoices = new List<string>();
        voiceLines = new Queue<AudioClip>();
        wordToChoiceIndex = new Dictionary<string, int>();
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
                if (wordToChoiceIndex.ContainsKey(word))
                {
                    HyperInkWrapper.instance.Choose(wordToChoiceIndex[word]);
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
        string continueAccumulator = "";

        if (HyperInkWrapper.instance.CanContinue())
        {
            wordToChoiceIndex.Clear();

            continueAccumulator += HyperInkWrapper.instance.Continue();
            checkTags();
        }
        continueAccumulator = DetermineChoices(continueAccumulator, HyperInkWrapper.instance.GetChoices());

        continueAccumulator += "\n";
        writing.text += continueAccumulator;

        choiceParent.Populate(currChoices.ToArray());
    }

    private string DetermineChoices(string textInput, string[] choices)
    {
        currChoices.Clear();

        string toReturn = textInput;

        for (int i = 0; i < choices.Length; i++)
        {
            if(choices[i][0] == HypertextSymbol)
            {
                //then it's a hypertext option
                string cleanedChoice = choices[i].Substring(1, choices[i].Length - 1).Trim();

                if (textInput.Contains(cleanedChoice)) //TODO make case insensitive
                {

                }
                else
                {
                    Debug.LogError("You wrote a hypertext choice that I couldn't find in the text: " + cleanedChoice);
                }

            } else
            {
                //then it's a regular choice
                currChoices.Add(choices[i]);
            }
        }

        return toReturn;
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

}
