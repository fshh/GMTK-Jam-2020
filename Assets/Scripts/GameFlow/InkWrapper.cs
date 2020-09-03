using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;

public class InkWrapper : MonoBehaviour
{
public TextAsset inkJSON;

    public Story story;

    bool responded = false;
    bool hasResponse;

    private string response;
    public string Response
    {
        get { return response; }
        set { response = value; hasResponse = true; }
    }


    bool typing = true;

    bool playingCoroutine = false;

    [Header("Time per message")]
    public float min;
    public float max;
    public float secPerLetter;


    public TextMeshProUGUI typingText;
    public Image subtitleBackground;
    public TextMeshProUGUI subtitles;

    public TextMeshProUGUI terminal;

    public VoiceLinesDictionary voiceOver;

    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSON.text);
        terminal.text = "";
    }                      

    // Update is called once per frame
    void Update()
    {
        if (hasResponse && Input.GetKeyDown(KeyCode.Return) && !story.canContinue)
        {
            responded = true;
        }

        if (story.canContinue)
        {
            if (!playingCoroutine)
            {
                StartCoroutine(pastingText(story.Continue()));
                processTags( story.currentTags.ToArray());
            }
        }
        else if (responded)
        {
            int choice = story.currentChoices.Count - 1;
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                if (response.ToLower().Contains(story.currentChoices[i].text.ToLower()))
                {
                    choice = i;
                }
            }

            story.ChooseChoiceIndex(choice);

            responded = false;
            hasResponse = false;
            response = "";
            typing = true;
        }
    }

    public void processTags(string [] tags)
    {
        if(tags.Length > 0)
        {
            string voiceLineTag = tags[0];

            string[] fields = voiceLineTag.Split(' ');

            string voiceLineName = fields[0];

            //TODO, more parsing

            voiceOver.playLine(voiceLineName);
        }
    }

    public bool isNumber(char possibleNumber)
    {
        return possibleNumber > '0' && possibleNumber < '9';
    }

    IEnumerator pastingText(string text)
    {
        playingCoroutine = true;
        typingText.enabled = true;
        yield return new WaitForSeconds(Mathf.Clamp(secPerLetter * text.Length, min, max));
        typingText.enabled = false;
        terminal.text += "\n" + text;
        typing = false;
        playingCoroutine = false;
    }

}
