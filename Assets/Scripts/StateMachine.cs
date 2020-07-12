using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class StateMachine : MonoBehaviour
{
    public enum state { User, Choosing, Voice, Typing};

    public state gameState;

    bool playingCoroutine = false;

    public TSVParser parser;

    public int line = 0;

    public float secPerLetter;

    public voiceLines voice;

    bool pressedEnter = false;

    public TextMeshProUGUI typing;
    public TextMeshProUGUI subtitles;

    // Update is called once per frame
    void Update()
    {
        if (line >= parser.wordList.Count - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        switch (gameState)
        {
            case state.User:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    gameState = state.Choosing;
                }
                break;
            case state.Choosing:

                if (parser.isSpoken[line])
                {
                    gameState = state.Voice;
                } else
                {
                    gameState = state.Typing;
                }
                break;
            case state.Voice:
                if (!playingCoroutine)
                {
                    StartCoroutine(playVO());
                }
                break;
            case state.Typing:
                if (!playingCoroutine)
                {
                    StartCoroutine(pastingText());
                }
                break;
        }

    }

    IEnumerator playVO()
    {
        playingCoroutine = true;
        subtitles.text = parser.wordList[line];
        yield return new WaitForSeconds(voice.playNextLine() + 1f);
        gameState = state.Choosing;
        playingCoroutine = false;
        line++;
    }

    IEnumerator pastingText()
    {
        playingCoroutine = true;
        typing.enabled = true;
        yield return new WaitForSeconds(secPerLetter * parser.wordList[line].Length);
        typing.enabled = false;
        parser.addText(line);
        gameState = state.User;
        playingCoroutine = false;
        line++;
    }
    
}
