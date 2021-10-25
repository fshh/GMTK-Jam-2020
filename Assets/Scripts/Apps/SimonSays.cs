using System.Collections;
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

	private enum SimonColor
	{
		Orange,
		Green,
		Pink,
		Blue,
		None
	}

	private SimonColor trickColor = SimonColor.None;
	public string TrickColor
	{
		set
		{
			switch (value.ToLower())
			{
				case "orange":
					trickColor = SimonColor.Orange;
					Debug.Log("Orange");
					break;
				case "green":
					trickColor = SimonColor.Green;
					Debug.Log("Green");
					break;
				case "pink":
					trickColor = SimonColor.Pink;
					Debug.Log("Pink");
					break;
				case "blue":
					trickColor = SimonColor.Blue;
					Debug.Log("Blue");
					break;
			}
		}
	}

	public TextMeshProUGUI scoreCounter;
	private int score = 0;

	// Start is called before the first frame update
	void Awake()
	{
		sequence = new List<int>();
	}

	private void SetScoreText()
	{
		scoreCounter.text = "Score: " + score.ToString();
	}

	public void ButtonInput(int buttonID)
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
				score += 100;
				SetScoreText();
				if (sequence.Count <= currentlyOn)
				{
					Clarity.Instance.ChooseByWord("won");
				}
			}
			else
			{
				//if incorrect, show visually, then reset the sequence list and start over (maybe call an event?)
				Clarity.Instance.ChooseByWord("lost");
				sequence.Clear();
				currentlyOn = 0;
			}
		}

		if (((SimonColor)buttonID).Equals(trickColor))
		{
			Clarity.Instance.ChooseByWord("trick");
			trickColor = SimonColor.None;
			sequence.Clear();
			currentlyOn = 0;
		}

	}

	public void CreateSequence(int length)
	{
		sequence.Clear();
		currentlyOn = 0;
		for (int i = 0; i < length; i++)
		{
			AddToSequence();
		}

		StartCoroutine(ShowSequence());
	}

	private IEnumerator ShowSequence()
	{
		//disable buttons
		ButtonsInteractable(false);

		foreach (int num in sequence)
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

		ButtonsInteractable(true);
	}

	private void ButtonsInteractable(bool state)
	{
		foreach (Button button in buttons)
		{
			button.interactable = state;
		}
	}

	private void AddToSequence()
	{
		sequence.Add(Random.Range(0, buttons.Count));
	}
}
