using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

[RequireComponent(typeof(AudioSource))]
public class Clarity : Singleton<Clarity>
{
    #region variables
    [Header("References")]
    public ClarityText clarityText;
    public ChoiceParent choiceParent;
    public GameObject popupPrefab, inputPopupPrefab;
    private GameObject popupParent;
    private Camera mainCamera;

    private bool waiting = true;
    public bool Waiting { set { waiting = value; } }

    public ApplicationSO simonApp;
    //public SimonSays simon;
    public ApplicationSO progressBarApp;
    private List<string> protectedChoices;

    /// <summary> Keeps track of which timedChoice we're on, won't do old ones. Easier than using stop coroutine! </summary>
    private int choiceID = 0;

    //Data structures and private references
    private Dictionary<string, int> wordToChoiceIndex, invisibleChoices;
    private Queue<AudioClip> voiceLines;
    private List<string> currChoices;
    private AudioSource clarityVoice; //The reason that audiosource is a required component

    //Magic number-y things
    private static string VO_FILE_PATH = "VO/";
    public static float WAIT_TIME = 2.0f;

    private static char HypertextSymbol = '^';
    private static char AutoSelectSymbol = '`';
    private static string VO_TAG = "VO: ";
    private static string DELETE_TAG = "delete: ";
    private static string WAIT_TAG = "wait: ";
    private static string SIMON_TAG = "simon: ";
    private static string POPUP_TAG = "popup: ";
    private static string INPUT_POPUP_TAG = "inputPopup: ";
    private static string PROGRESS_BAR_TAG = "progressbar: ";
    #endregion
    private void Awake()
    {
        clarityVoice = GetComponent<AudioSource>();
        mainCamera = Camera.main;
        popupParent = GameObject.FindGameObjectWithTag("PopupParent");

        currChoices = new List<string>();
        protectedChoices = new List<string>();
        voiceLines = new Queue<AudioClip>();
        wordToChoiceIndex = new Dictionary<string, int>();
        invisibleChoices = new Dictionary<string, int>();
    }

    public void Start()
    {
        int nextChoice = SaveWrapper.Instance.LoadStory();
        if (nextChoice >= 0)
        {
            Choose(nextChoice);
        }
        else
        {
            ContinueUntilChoice();
        }

        CommandLine.instance.commands["go"] += ToKnot;
    }


    private void Update()
    {
        CheckForNextVoiceClip();
    }

    private void OnDestroy()
    {
        CommandLine.instance.commands["go"] -= ToKnot;
    }

    private void ToKnot(string knotName)
    {
        HyperInkWrapper.instance.GoToKnot(knotName);
        Debug.Log($"Jumped to knot '{knotName}'.");
        ContinueUntilChoice();
    }

    /// <summary>
    /// Function specifically for choosing "invisible" protected choices, like for clarity says choosing "won" or "lost"
    /// </summary>
    public void ChooseByWord(string word)
    {
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
        protectedChoices.Clear();
        choiceParent.Clear();
        clarityText.ClearWordButtons();

        while (HyperInkWrapper.instance.CanContinue())
        {
            AudioTags();
            DeleteTags();
            GameTags();
            yield return new WaitForSeconds(WaitTags());
            yield return clarityText.AddText(HyperInkWrapper.instance.Continue(), waiting);
        }
        PopupTags();
        DetermineChoices(HyperInkWrapper.instance.GetChoices());
        clarityText.Enter();

        choiceParent.Populate(currChoices.ToArray());
    }

    /// <summary>
    /// Important - the string this returns is supposed to be the new version of the textInput param
    /// </summary>
    /// <param name="textInput"></param>
    /// <param name="choices"></param>
    /// <returns>textInput with highlights added from the hypertext choices</returns>
    private void DetermineChoices(string[] choices)
    {
        currChoices.Clear(); //Housekeeping

        for (int i = 0; i < choices.Length; i++)
        {
            string cleanedChoice = choices[i];

            if (cleanedChoice[0] == AutoSelectSymbol)
            {
                string[] tempSplit = cleanedChoice.Split(AutoSelectSymbol);
                if (tempSplit.Length < 2)
                {
                    Debug.LogError("confused what to do here, were you trying to write a timed option?");
                }
                else
                {
                    string s_time = tempSplit[1];
                    float time = float.Parse(s_time);

                    StartCoroutine(TimedChoice(i, time));
                    if (!tempSplit[2].Equals(""))
                    {
                        cleanedChoice = tempSplit[2]; //Regular timed choice
                    }
                    else
                    {
                        continue; //Invisible timed choice (just gave a number with no letters to go along with it)
                    }
                }
            }

            bool invisible = false;
            foreach (var protectedChoice in protectedChoices.Where(protectedChoice => protectedChoice.ToUpper().Equals(cleanedChoice.ToUpper())))
            {
                invisible = true; //if a protected choice, don't list it
            }

            if (cleanedChoice[0] == HypertextSymbol && !invisible)//then it's a hypertext option
            {
                cleanedChoice = cleanedChoice.Substring(1, cleanedChoice.Length - 1).Trim(); //lop off first character

                if (clarityText.Contains(cleanedChoice)) //TODO make case insensitive
                {
                    invisibleChoices.Add(cleanedChoice, i);
                    clarityText.Hypertextify(cleanedChoice);
                }
                else
                {
                    Debug.LogError("You wrote a hypertext choice that I couldn't find in the text: " + cleanedChoice);
                }
            }
            else if (invisible)
            {
                invisibleChoices.Add(cleanedChoice, i);
            }
            else
            {
                //then it's a regular choice, add it as such
                currChoices.Add(cleanedChoice);
            }
        }
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
    /// Plays audio related to any audio tags
    /// </summary>
    private void AudioTags()
    {
        string[] tags = HyperInkWrapper.instance.getTags();

        foreach (string tag in tags)
        {
            if (tag.Contains(VO_TAG))
            {
                AudioClip toAdd = (AudioClip)Resources.Load(VO_FILE_PATH + tag.Replace(VO_TAG, ""));
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
        if (!waiting)
        {
            return 0;
        }

        float accumulator = 0;
        IEnumerable<string> waitTags = HyperInkWrapper.instance.getTags().Where(tag => tag.Contains(WAIT_TAG));
        if (!waitTags.Any())
        {
            return WAIT_TIME;
        }

        foreach (string tag in waitTags)
        {
            accumulator += float.Parse(tag.Replace(WAIT_TAG, "").Trim());
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

                clarityText.DeletePrevious(start, end);
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
            if (!tag.Contains(SIMON_TAG)) continue;

            string[] simonArgs = tag.Replace(SIMON_TAG, "").Trim().Split(',');
            if (simonArgs[0].Trim().ToUpper().Equals("start".ToUpper()))
            {
                Window simonWindow = simonApp.OpenWindow();
                SimonSays simon = simonWindow.content.GetComponent<SimonSays>();

                simon.CreateSequence(int.Parse(simonArgs[1].Trim()));
                protectedChoices.Add("won");
                protectedChoices.Add("lost");
            }

            if (simonArgs[0].Trim().ToUpper().Equals("trick".ToUpper()))
            {
                Window simonWindow = simonApp.OpenWindow();
                SimonSays simon = simonWindow.content.GetComponent<SimonSays>();

                simon.TrickColor = simonArgs[1].Trim();
                protectedChoices.Add("trick");
            }
        }
    }

    private void PopupTags()
    {
        string[] tags = HyperInkWrapper.instance.getTags();
        foreach (string tag in tags)
        {
            if (tag.Contains(POPUP_TAG))
            {
                string[] args = tag.Replace(POPUP_TAG, "").Trim().Split(',');

                if (args.Length >= 4)
                {
                    GameObject popup = Instantiate(popupPrefab, popupParent.transform);
                    popup.GetComponent<Popup>().Init(args[0].Trim(), args[1].Trim(), args[2].Trim(), args[3].Trim());
                    protectedChoices.Add(args[2].Trim());
                    protectedChoices.Add(args[3].Trim());
                }
                else
                {
                    Debug.Log("You need more args to call the popup function. Try popup: <description>, <extraDescription>, <ButtonOneText>, <ButtonTwoText>");
                }
            }

            if (tag.Contains(INPUT_POPUP_TAG))
            {
                string[] args = tag.Replace(INPUT_POPUP_TAG, "").Trim().Split(',');

                if (args.Length >= 2)
                {
                    GameObject popup = Instantiate(inputPopupPrefab, popupParent.transform);
                    bool shouldBeLowercase = args.Length >= 3 ? bool.Parse(args[2].Trim()) : false;
                    protectedChoices.Add(popup.GetComponent<InputPopup>().Init(args[0].Trim(), args[1].Trim(), shouldBeLowercase));
                }
                else
                {
                    Debug.Log("You need more args to call the inputPopup function. Try inputPopup: <description>, <inkVariableName>, optional: <shouldBeLowercase>");
                }
            }

            if (tag.Contains(PROGRESS_BAR_TAG))
            {
                string[] args = tag.Replace(PROGRESS_BAR_TAG, "").Trim().Split(',');

                if (args.Length >= 3)
                {
                    Window progressBarWindow = progressBarApp.OpenWindow();

                    bool appearOnTop = bool.Parse(args[1].Trim());
                    if (!appearOnTop)
                    {
                        WindowManager.Instance.SendWindowToBack(progressBarWindow);
                    }

                    ProgressBar progressBar = progressBarWindow.content.GetComponent<ProgressBar>();
                    progressBar.Init(float.Parse(args[0].Trim()), bool.Parse(args[2].Trim()));
                }
                else
                {
                    Debug.Log("You need more args to call the progressbar function. Try progressbar: <timeToComplete>, <appearOnTop>, <isCancelable>");
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
        SaveWrapper.Instance.SaveStory(choice);
        HyperInkWrapper.instance.Choose(choice);
        ContinueUntilChoice();
    }
}
