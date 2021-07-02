using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

[RequireComponent(typeof(AudioSource))]
public class Clarity : Singleton<Clarity>
{
    #region variables
    [Header("References")]
    public TextMeshProUGUI writing;
    public Camera mainCamera;
    public ChoiceParent choiceParent;

    [Header("TODO: make serialized")]
    public List<string> protectedChoices;
    public SimonSays simon;

    /// <summary> Keeps track of which timedChoice we're on, won't do old ones. Easier than using stop coroutine! </summary>
    private int choiceID = 0;

    //Data structures and private references
    private Dictionary<string, int> wordToChoiceIndex, invisibleChoices;
    private Queue<AudioClip> voiceLines;
    private List<string> currChoices;
    private AudioSource clarityVoice; //The reason that audiosource is a required component

    //Magic number-y things
    private static string AUDIO_FILE_PATH = "Wav Files/VO/";
    private static string highlightPrefix = "<mark=#ccff0033>";//"<mark=#22222233>";
    private static string highlightSuffix = "</mark>";

    private static char HypertextSymbol = '^';
    private static char AutoSelectSymbol = '`';
    private static string AUDIO_TAG = "audio: ";
    private static string DELETE_TAG = "delete: ";
    private static string WAIT_TAG = "wait: ";
    private static string SIMON_TAG = "simon: ";
    #endregion


    private void Awake()
    {
        clarityVoice = GetComponent<AudioSource>();
        currChoices = new List<string>();
        voiceLines = new Queue<AudioClip>();
        wordToChoiceIndex = new Dictionary<string, int>();
        invisibleChoices = new Dictionary<string, int>();
    }

    public void Start()
    {
        ContinueUntilChoice();
    }

    private void Update()
    {
        CheckForClickedHypertext();
        CheckForNextVoiceClip();
    }

    [Button]
    //TODO delete this debugging function
    public void chooseWin()
    {
        chooseByWord("won");
    }

    /// <summary>
    /// Function specifically for choosing "invisible" protected choices, like for clarity says choosign "won" or "lost"
    /// </summary>
    public void chooseByWord(string word)
    {
        //TODO make case insensitive
        if (!protectedChoices.Contains(word))
        {
            Debug.Log("No such protected choice exists: " + word);
        }

        if (invisibleChoices.ContainsKey(word))
        {
            Choose(invisibleChoices[word]);
        }
        else
        {
            Debug.Log("No such invisible choice exists: " + word);
        }
    }

    /// <summary>
    /// For calling from Update: checks if the player has clicked one of the hypertext words
    /// </summary>
    private void CheckForClickedHypertext()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var wordIndex = TMP_TextUtilities.FindNearestWord(writing, Input.mousePosition, mainCamera);


            //word must exist, but also the cursor must be over the text box, otherwise you can click from far away
            //TODO big white spaces in the text box can still be tricky, may want to fix later.
            if (wordIndex != -1 && TMP_TextUtilities.IsIntersectingRectTransform(writing.rectTransform, Input.mousePosition, Camera.main))
            {
                string word = writing.textInfo.wordInfo[wordIndex].GetWord();

                foreach (KeyValuePair<string, int> pair in wordToChoiceIndex)
                {
                    if (pair.Key.Contains(word))
                    {
                        Choose(pair.Value);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// For calling from Update: Plays next voice clip if there's one to play and nothing is currently being played
    /// </summary>
    private void CheckForNextVoiceClip()
    {
        //dumb implementation of voice playing, probably want to include a pause later (Ezra)
        if (!clarityVoice.isPlaying && voiceLines.Count > 0)
        {
            clarityVoice.clip = voiceLines.Dequeue();
            clarityVoice.Play();
        }
    }

    /// <summary>
    /// Processes text from here until the next choice
    /// </summary>
    private void ContinueUntilChoice()
    {
        StartCoroutine(ContinueUntilChoiceHelper());
    }

    private IEnumerator ContinueUntilChoiceHelper()
    {
        wordToChoiceIndex.Clear(); //Housekeeping
        invisibleChoices.Clear();

        //Remove highlights
        writing.text = writing.text.Replace(highlightPrefix, "");
        writing.text = writing.text.Replace(highlightSuffix, "");

        while (HyperInkWrapper.instance.CanContinue())
        {
            AudioTags();
            DeleteTags();
            GameTags();
            yield return new WaitForSeconds(WaitTags());
            writing.text += HyperInkWrapper.instance.Continue();
        }
        writing.text = DetermineChoices(writing.text, HyperInkWrapper.instance.GetChoices()) + "\n";

        choiceParent.Populate(currChoices.ToArray());
    }

    /// <summary>
    /// Important - the string this returns is supposed to be the new version of the textInput param
    /// </summary>
    /// <param name="textInput"></param>
    /// <param name="choices"></param>
    /// <returns>textInput with highlights added from the hypertext choices</returns>
    private string DetermineChoices(string textInput, string[] choices)
    {
        currChoices.Clear(); //Housekeeping
        string toReturn = textInput;

        for (int i = 0; i < choices.Length; i++)
        {
            string cleanedChoice = choices[i];

            if (cleanedChoice[0] == AutoSelectSymbol)
            {
                string[] tempSplit = cleanedChoice.Split(AutoSelectSymbol);
                if(tempSplit.Length < 3)
                {
                    Debug.LogError("confused what to do here, were you trying to write a timed option?");
                }
                else
                {
                    cleanedChoice = tempSplit[2];
                    string s_time = tempSplit[1];
                    float time = float.Parse(s_time);

                    StartCoroutine(TimedChoice(i, time));
                }
            }

            bool invisible = false;
            foreach (string protectedChoice in protectedChoices)
            {
                if (protectedChoice.ToUpper().Equals(cleanedChoice.ToUpper()))
                {
                    invisible = true; //if a protected choice, don't list it
                }
            }

            if (cleanedChoice[0] == HypertextSymbol && !invisible)//then it's a hypertext option
            {
                cleanedChoice = cleanedChoice.Substring(1, cleanedChoice.Length - 1).Trim(); //lop off first character

                if (toReturn.Contains(cleanedChoice)) //TODO make case insensitive
                {
                    wordToChoiceIndex.Add(cleanedChoice, i);
                    toReturn = Hypertextify(toReturn, cleanedChoice);
                }
                else
                {
                    Debug.LogError("You wrote a hypertext choice that I couldn't find in the text: " + cleanedChoice);
                }
            } else if (invisible)
            {
                invisibleChoices.Add(cleanedChoice, i);
            }
            else
            {
                //then it's a regular choice, add it as such
                currChoices.Add(cleanedChoice);
            }
        }

        return toReturn;
    }

    /// <summary>
    /// Note: to function properly must have something else stop this coroutine if another choice is made
    /// </summary>
    /// <param name="choice"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator TimedChoice(int choice, float time)
    {
        //Store what choice we're currently on, Choose should increment this value
        int ID = choiceID;
        yield return new WaitForSeconds(time);

        //If we're still on the right branch, then choose
        if (ID == choiceID)
        {
            Choose(choice);
        }
    }


    //TODO fix to work with the new tagging system - currently broken!
    /// <summary>
    /// Plays audio related to any audio tags (TODO add other tag parsing, currently only works with audio)
    /// </summary>
    private void AudioTags()
    {
        string[] tags = HyperInkWrapper.instance.getTags();

        foreach(string tag in tags)
        {
            if (tag.Contains(AUDIO_TAG))
            {
                AudioClip toAdd = (AudioClip)Resources.Load(AUDIO_FILE_PATH + tag.Replace(AUDIO_TAG, ""));
                if (toAdd == null)
                {
                    Debug.LogError("Incorrect VO Tag, was unable to find " + tag + " in our resources folder :/ (Ezra)");
                }
                else
                {
                    voiceLines.Enqueue(toAdd);
                }
            }
        }
    }

    /// <summary>
    /// Returns the sum of all arguments to wait tags, rounded up to at least 0
    /// </summary>
    /// <returns>Time the program should wait before continuing</returns>
    private float WaitTags()
    {
        float accumulator = 0;
        string[] tags = HyperInkWrapper.instance.getTags();

        foreach (string tag in tags)
        {
            if (tag.Contains(WAIT_TAG))
            {
                accumulator += int.Parse(tag.Replace(WAIT_TAG, "").Trim());
            }
        }

        return Mathf.Max(accumulator, 0);
    }

    private void DeleteTags()
    {
        string[] tags = HyperInkWrapper.instance.getTags();

        foreach (string tag in tags)
        {
            if (tag.Contains(DELETE_TAG))
            {
                string[] startEndStrings = tag.Replace(DELETE_TAG, "").Trim().Split(',');
                string start = startEndStrings[0].Trim();
                string end = startEndStrings[1].Trim();

                DeletePrevious(start, end);
            }
        }
    }

    /// <summary>
    /// Parses tags of the form SIMON_TAG command, length
    /// </summary>
    private void GameTags()
    {
        string[] tags = HyperInkWrapper.instance.getTags();

        foreach (string tag in tags)
        {
            if (tag.Contains(SIMON_TAG))
            {
                string[] simonArgs = tag.Replace(SIMON_TAG, "").Trim().Split(',');
                if (simonArgs[0].Trim().ToUpper().Equals("start".ToUpper()))
                {
                    simon.createSequence(int.Parse(simonArgs[1].Trim()));
                }
            }
        }
    }

    /// <summary>
    /// Calls choose from HyperInkWrapper (passing choice), increments choiceID
    /// </summary>
    /// <param name="choice"></param>
    public void Choose(int choice)
    {
        choiceID++;
        choiceParent.Clear();
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

        string parsed = highlightPrefix + parse + highlightSuffix;

        return beforeParse + parsed + afterParse;
    }

    //This code is wildly inefficient but I don't think it should matter, it's not called often
    //might be wonky with rich text, this is a first implementation
    #region deletions
    public void DeletePrevious(string startString, string endString)
    {
        if(!writing.text.Contains(startString))
        {
            Debug.LogError("Couldn't find string: " + startString);
            return;
        }
        else if(!writing.text.Contains(endString))
        {
            Debug.LogError("Couldn't find string: " + endString);
            return;
        }
        else
        {

            string[] splitByEndString = SplitStringWithString(writing.text, endString);

            string beforeEndString = "";

            //minus one because we don't want to include the last bit
            for(int i = 0; i < splitByEndString.Length - 1; i++)
            {
                beforeEndString += splitByEndString[i];
            }

            string[] splitByStartString = SplitStringWithString(beforeEndString, startString);

            string middleToDelete = splitByStartString[splitByStartString.Length - 1];

            //Actual deletions
            string newWritingString = writing.text.Replace(startString, "");
            newWritingString = newWritingString.Replace(middleToDelete, "");
            newWritingString = newWritingString.Replace(endString, "");

            writing.text = newWritingString;
        }
    }

    /// <summary>
    /// requires that there be no ` in the original text
    /// </summary>
    /// <param name="overall"></param>
    /// <param name="splitter"></param>
    /// <returns>strings split by but not including the splitter param</returns>
    public string[] SplitStringWithString(string overall, string splitter)
    {
        //bit of a hack, first I replace the string with a char, then split with the new char
        overall = overall.Replace(splitter, "`");

        return overall.Split('`');
    }
    #endregion
}
