using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ClarityText : MonoBehaviour
{
    private TextMeshProUGUI output;
    public string Text
    {
        set { output.text = value; }
        get { return output.text; }
    }

    public float BUTTON_MARGIN_X = 0, BUTTON_MARGIN_Y = 0;
    public GameObject wordButtonPrefab, wordButtonParent;
    private List<GameObject> wordButtons;
    public SFXPlayer sfx;

    [SerializeField]
    private int lettersUntilSound = 5;

    public float timeBetweenLetters = 0.03f, variance = 0.01f;

    private CanvasScaler canvasScaler;

    // Start is called before the first frame update
    void Awake()
    {
        output = GetComponent<TextMeshProUGUI>();
        wordButtons = new List<GameObject>();
        canvasScaler = FindObjectOfType<CanvasScaler>();
    }

    public IEnumerator AddText(string newText, bool waiting)
    {
        int soundCounter = 0;
        if (waiting)
        {
            for (int i = 0; i < newText.Length; i++)
            {
                float waitTime = Mathf.Max(timeBetweenLetters + Random.Range(-variance, variance));
                yield return new WaitForSeconds(waitTime);

                //this check is a hack to try and avoid the case where you use < but not in a rich text way. really only works for <br> currently
                //https://twitter.com/ESzanton/status/1433541156667895812?s=20 
                int maxRichTextLength = 4;
                string stringToCheck = "";

                if (newText.Length - i >= maxRichTextLength)
                {
                    stringToCheck = newText.Substring(i, maxRichTextLength);
                }
                if (newText[i] == '<' && stringToCheck.Contains(">"))
                {
                    int lengthOfRichText = stringToCheck.IndexOf('>');

                    string addOn = newText.Substring(i, lengthOfRichText);
                    output.text += addOn;
                    i += lengthOfRichText;
                }

                output.text += newText[i];
                soundCounter++;
                //if (soundCounter >= lettersUntilSound) Old code
                if (output.text[output.text.Length - 1].Equals(' ') || output.text[output.text.Length - 1].Equals('\n'))
                {
                    soundCounter = 0;
                    sfx.PlayRandom();
                }
            }
        }
        else
        {
            output.text += newText;
            yield return new WaitForEndOfFrame();
        }
    }

    public void Enter()
    {
        output.text += "\n";
    }

    public bool Contains(string queryString)
    {
        return output.text.Contains(queryString);
    }

    //I'm doing this in an inefficient way first, may want to change to stringbuilder later (Ezra)
    public void Hypertextify(string wordToHypertext)
    {
        int firstIndex = output.text.LastIndexOf(wordToHypertext);

        if (firstIndex == -1) //then it wasn't found, return unparsed input
        {
            Debug.LogError("are you sure you meant to call hypertextify? seems like that word wasn't in the text body: " + wordToHypertext);
            return;
        }

        SetWordButtonLocation(wordToHypertext);
    }

    public void SetWordButtonLocation(string toMakeButton)
    {
        TMP_TextInfo textInfo = output.textInfo;

        Vector2Int characters = FindString(textInfo, toMakeButton);
        int firstCharacter = characters[0];
        int lastCharacter = characters[1];

        float maxAscender = Mathf.NegativeInfinity;
        float minDescender = Mathf.Infinity;
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;

        // Iterate through each character of the word
        for (int characterIndex = firstCharacter; characterIndex < lastCharacter; characterIndex++)
        {
            TMP_CharacterInfo currentCharInfo = textInfo.characterInfo[characterIndex];

            // Track Max Ascender and Min Descender
            maxAscender = Mathf.Max(maxAscender, currentCharInfo.ascender);
            minDescender = Mathf.Min(minDescender, currentCharInfo.descender);
            minX = Mathf.Min(minX, currentCharInfo.bottomLeft.x);
            maxX = Mathf.Max(maxX, currentCharInfo.bottomRight.x);
        }

        GameObject newButton = Instantiate(wordButtonPrefab, wordButtonParent.transform);
        wordButtons.Add(newButton);
        newButton.GetComponent<WordButton>().choiceString = toMakeButton;

        // (Andrew) NOTE: not sure if the canvasScaler stuff actually works here since currently the scaleFactor is 1.
        // a forum post said I should do it so I have it here as a placeholder. I don't think our canvas scaling will change from 1 so hopefully doesn't matter.
        Vector3 bottomLeft = wordButtonParent.transform.InverseTransformPoint(output.transform.TransformPoint(new Vector3(minX, minDescender)) / canvasScaler.scaleFactor);
        Vector3 topRight = wordButtonParent.transform.InverseTransformPoint(output.transform.TransformPoint(new Vector3(maxX, maxAscender)) / canvasScaler.scaleFactor);
        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        RectTransform rt = newButton.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width + BUTTON_MARGIN_X, height + BUTTON_MARGIN_Y);
        rt.anchoredPosition = bottomLeft + new Vector3(width / 2.0f, height / 2.0f, 0);
    }

    public void ClearWordButtons()
    {
        foreach (GameObject wordButton in wordButtons)
        {
            Destroy(wordButton);
        }
        wordButtons.Clear();
    }

    //TODO make less horribly inefficient
    public Vector2Int FindString(TMP_TextInfo textInfo, string toFind)
    {
        // (Andrew) tried doing this with just textInfo.textComponent.text, but the indices are all offset if I do that, so this'll do for now
        // if this gets too slow later as the full text gets longer and longer, we can look into a better way to resolve that offset
        string fullString = new string(textInfo.characterInfo.Select(ci => ci.character).ToArray());

        int first = fullString.LastIndexOf(toFind);
        int last = first + toFind.Length;

        return new Vector2Int(first, last);
    }

    //This code is wildly inefficient but I don't think it should matter, it's not called often
    //might be wonky with rich text, this is a first implementation
#region deletions
    public void DeletePrevious(string startString, string endString)
    {
        if (!output.text.Contains(startString))
        {
            Debug.LogError("Couldn't find string: " + startString);
            return;
        }
        else if (!output.text.Contains(endString))
        {
            Debug.LogError("Couldn't find string: " + endString);
            return;
        }
        else
        {

            string[] splitByEndString = SplitStringWithString(output.text, endString);

            string beforeEndString = "";

            //minus one because we don't want to include the last bit
            for (int i = 0; i < splitByEndString.Length - 1; i++)
            {
                beforeEndString += splitByEndString[i];
            }

            string[] splitByStartString = SplitStringWithString(beforeEndString, startString);

            string middleToDelete = splitByStartString[splitByStartString.Length - 1];

            //Actual deletions
            string newWritingString = output.text.Replace(startString, "");
            newWritingString = newWritingString.Replace(middleToDelete, "");
            newWritingString = newWritingString.Replace(endString, "");

            output.text = newWritingString;
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
