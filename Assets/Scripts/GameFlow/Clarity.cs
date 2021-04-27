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

    private static string highlightPrefix = "<mark=#22222233>";
    private static string highlightSuffix = "</mark>";

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
                foreach(KeyValuePair<string, int> pair in wordToChoiceIndex)
                {
                    if (pair.Key.Contains(word))
                    {
                        HyperInkWrapper.instance.Choose(pair.Value);
                        ContinueUntilChoice();
                        break;
                    }
                }

                //if (wordToChoiceIndex.ContainsKey(word))
                //{
                //    Debug.Log("choosing: " + wordToChoiceIndex[word]);
                //    HyperInkWrapper.instance.Choose(wordToChoiceIndex[word]);
                //    ContinueUntilChoice();
                //}
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
        StartCoroutine(ContinueUntilChoiceHelper());
    }

    private IEnumerator ContinueUntilChoiceHelper()
    {
        wordToChoiceIndex.Clear();
        //remove highilights
        writing.text = writing.text.Replace(highlightPrefix, "");
        writing.text = writing.text.Replace(highlightSuffix, "");

        string textBeforeContinue = writing.text;

        string continueAccumulator = "";

        while (HyperInkWrapper.instance.CanContinue())
        {
            continueAccumulator += HyperInkWrapper.instance.Continue();
            Debug.Log("accumulator: " + continueAccumulator);
            checkTags();
            yield return new WaitUntil(HyperInkWrapper.instance.WaitDelegate);
            writing.text = textBeforeContinue + continueAccumulator;
        }
        continueAccumulator = DetermineChoices(continueAccumulator, HyperInkWrapper.instance.GetChoices());


        //add new text
        continueAccumulator += "\n";
        writing.text = textBeforeContinue + continueAccumulator;

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

                if (toReturn.Contains(cleanedChoice)) //TODO make case insensitive
                {
                    wordToChoiceIndex.Add(cleanedChoice, i);
                    toReturn = Hypertextify(toReturn, cleanedChoice);
                }
                else
                {
                    Debug.LogError("You wrote a hypertext choice that I couldn't find in the text: " + cleanedChoice);
                }

            } else
            {
                //then it's a regular choice, add it as such
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
            //Debug.Log(tag);
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
    public string Hypertextify(string bodyText, string wordToHypertext)
    {
        int firstIndex = bodyText.IndexOf(wordToHypertext);

        if (firstIndex == -1) //then it wasn't found, return unparsed input
        {
            Debug.LogError("are you sure you meant to call hypertextify? seems like that word wasn't in the text body: " + wordToHypertext);
            return bodyText;
        }

        string beforeParse = bodyText.Substring(0, firstIndex);
        string parse = wordToHypertext;
        string afterParse = bodyText.Substring(firstIndex + wordToHypertext.Length);

        //string afterParse;
        //if (firstSpace == -1) //if no spaces, then it was the last word
        //{
        //    afterParse = "";
        //}
        //else
        //{
        //    afterParse = toParse.Substring(firstSpace);
        //    toParse = toParse.Substring(0, firstSpace);
        //}


        //string[] wordAndKnot = toParse.Split(HypertextSymbol); //1 is word, 2 is knot name (0 is an empty string)


        //wordToKnotName.Add(wordAndKnot[1], wordAndKnot[2]);
        string parsed = highlightPrefix + parse + highlightSuffix; //TODO add markup and stuff

        return beforeParse + parsed + afterParse;
    }
}
