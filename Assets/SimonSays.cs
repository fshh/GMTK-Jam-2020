﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;

public class SimonSays : MonoBehaviour
{
    public List<Button> buttons;

    public List<int> sequence;
    public int currentlyOn; //the next button the player is supposed to press

    public float buttonGlowTime;
    public float buttonPauseTime;

    public TextMeshProUGUI scoreCounter;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        sequence = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetScoreText()
    {
        scoreCounter.text = "Score: " + score.ToString();
    }

    public void buttonInput(int buttonID)
    {
        //if correct, validate visually then advance to the next number
        if (sequence.Count <= 0)
        {
            Debug.Log("I didn't tell you to click anything yet!");
        }
        else
        {
            if (buttonID == sequence[currentlyOn])
            {
                currentlyOn++;
                score = score + 100;
                SetScoreText();
                if (sequence.Count <= currentlyOn)
                {
                    Clarity.Instance.chooseByWord("won");
                }
            }
            else
            {
                //if incorrect, show visually, then reset the sequence list and start over (maybe call an event?)
                Clarity.Instance.chooseByWord("lost");
                sequence.Clear();
                currentlyOn = 0;
            }
        }

    }

    public void createSequence(int length)
    {
        Debug.Log("creating sequence of length: " + length);
        sequence.Clear();
        currentlyOn = 0;
        for(int i = 0; i < length; i++)
        {
            addToSequence();
        }

        StartCoroutine(showSequence());
    }

    public IEnumerator showSequence()
    {
        //disable buttons
        buttonsInteractable(false);

        foreach(int num in sequence)
        {
            Button button = buttons[num];
            yield return new WaitForSeconds(buttonPauseTime);
            ColorBlock colors = button.colors;
            Color temp = colors.disabledColor;
            colors.disabledColor = colors.pressedColor;
            button.colors = colors;
            yield return new WaitForSeconds(buttonGlowTime);
            colors.disabledColor = temp;
            button.colors = colors;
        }

        buttonsInteractable(true);
    }

    private void buttonsInteractable(bool state)
    {
        foreach (Button button in buttons)
        {
            button.interactable = state;
        }
    }

    private void addToSequence()
    {
        sequence.Add(Random.Range(0, buttons.Count));
    }
}
