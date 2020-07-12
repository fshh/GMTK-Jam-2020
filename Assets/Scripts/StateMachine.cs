using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum state { User, Choosing, Voice, Typing};

    public state gameState;

    bool playingCoroutine = false;

    public TSVParser parser;

    public int line = 0;

    public float secPerLetter;

    public voiceLines voice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        yield return new WaitForSeconds(voice.playNextLine() + 0.5f);
        gameState = state.Choosing;
        playingCoroutine = false;
        line++;
    }

    IEnumerator pastingText()
    {
        playingCoroutine = true;
        yield return new WaitForSeconds(secPerLetter * parser.wordList[line].Length);
        parser.addText(line);
        gameState = state.User;
        playingCoroutine = false;
        line++;
    }
    
}
