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


    bool typing =  true;

    bool playingCoroutine = false;

    public float secPerLetter;

    public TextMeshProUGUI typingText;
    public Image subtitleBackground;
    public TextMeshProUGUI subtitles;

    public TextMeshProUGUI terminal;

    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSON.text);
        terminal.text = "";
    }

    // Update is called once per frame
    void Update()
    {

        if(hasResponse && Input.GetKeyDown(KeyCode.Return))
        {
            responded = true;
        }

        //if (typing)
        //{
            if (!playingCoroutine)
            {
                if (story.canContinue)
                {
                    StartCoroutine(pastingText(story.Continue()));
                }
                else
                {
                    typing = false;
                }
            }
        //}
        
        if (responded)
        {
            int choice = story.currentChoices.Count - 1;
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Debug.Log(story.currentChoices[i].text);
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


    IEnumerator pastingText(string text)
    {
        playingCoroutine = true;
        typingText.enabled = true;
        yield return new WaitForSeconds(secPerLetter * text.Length);
        typingText.enabled = false;
        terminal.text += "\n \n" + text;
        typing = false;
        playingCoroutine = false;
    }

}
