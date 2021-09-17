using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class ClarityText : MonoBehaviour
{
    private TextMeshProUGUI output;
    private static float BUTTON_SIZE_MULTIPLIER_MAGIC_NUMBER = 108; //TODO figure out why this is the number and replace
    public float BUTTON_MARGIN_X = 0, BUTTON_MARGIN_Y = 0;
    public GameObject wordButtonPrefab, wordButtonParent;
    private List<GameObject> wordButtons;
    
    public float timeBetweenLetters = 0.03f, variance = 0.01f;
    // Start is called before the first frame update
    void Awake()
    {
        output = GetComponent<TextMeshProUGUI>();
        wordButtons = new List<GameObject>();
    }

    public IEnumerator AddText(string newText, bool notWaiting)
    {
        if (notWaiting)
        {
            output.text += newText;
            yield return new WaitForEndOfFrame();
        }
        else
        {
            for (int i = 0; i < newText.Length; i++)
            {
                float waitTime = Mathf.Max(timeBetweenLetters + Random.Range(-variance, variance));
                yield return new WaitForSeconds(waitTime);
            
                //this check is a hack to try and avoid the case where you use < but not in a rich text way. really only works for <br> currently
                //https://twitter.com/ESzanton/status/1433541156667895812?s=20 
                int maxRichTextLength = 4;
                string stringToCheck = "";
            
                if(newText.Length - i >= maxRichTextLength){
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
            }
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
            
            bool isBeginRegion = false;

            Vector3 bottomLeft = Vector3.zero;
            Vector3 topLeft = Vector3.zero;
            Vector3 bottomRight = Vector3.zero;
            Vector3 topRight = Vector3.zero;

            float maxAscender = -Mathf.Infinity;
            float minDescender = Mathf.Infinity;
            
            // Iterate through each character of the word
            for (int characterIndex = firstCharacter; characterIndex < lastCharacter; characterIndex++)
            {
                TMP_CharacterInfo currentCharInfo = textInfo.characterInfo[characterIndex];
                int currentLine = currentCharInfo.lineNumber;
                
                bool isCharacterVisible = characterIndex > output.maxVisibleCharacters ||
                                          currentCharInfo.lineNumber > output.maxVisibleLines ||
                                         (output.overflowMode == TextOverflowModes.Page && currentCharInfo.pageNumber + 1 != output.pageToDisplay) ? false : true;

                // Track Max Ascender and Min Descender
                maxAscender = Mathf.Max(maxAscender, currentCharInfo.ascender);
                minDescender = Mathf.Min(minDescender, currentCharInfo.descender);


                if (isBeginRegion == false && isCharacterVisible)
                {
                    isBeginRegion = true;

                    bottomLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.descender, 0);
                    topLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.ascender, 0);

                    // If Word is one character
                    if ((lastCharacter - firstCharacter) == 1)
                    {
                        isBeginRegion = false;

                        topLeft = transform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                        bottomLeft = transform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                        bottomRight = transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                        topRight = transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));
                    }
                }

                
                // Last Character of Word
                if (isBeginRegion && characterIndex == lastCharacter - 1)
                {
                    isBeginRegion = false;

                    topLeft = transform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                    bottomLeft = transform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                    bottomRight = transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                    topRight = transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));
                }
                // If Word is split on more than one line.
                /*else if (isBeginRegion && currentLine != textInfo.characterInfo[characterIndex + 1].lineNumber)
                {
                    isBeginRegion = false;

                    topLeft = transform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                    bottomLeft = transform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                    bottomRight = transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                    topRight = transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                    // Draw Region
                    DrawRectangle(bottomLeft, topLeft, topRight, bottomRight, wordColor);
                    //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                    maxAscender = -Mathf.Infinity;
                    minDescender = Mathf.Infinity;

                }*/
            }

            GameObject newButton = Instantiate(wordButtonPrefab, wordButtonParent.transform);
            wordButtons.Add(newButton);
            newButton.GetComponent<WordButton>().choiceString = toMakeButton;
            RectTransform rt = newButton.GetComponent<RectTransform>();
            float width = topRight.x - bottomLeft.x, height = topRight.y - bottomLeft.y;
            rt.sizeDelta = new Vector2(width + BUTTON_MARGIN_X, height + BUTTON_MARGIN_Y) * BUTTON_SIZE_MULTIPLIER_MAGIC_NUMBER;
            rt.transform.position = bottomLeft + new Vector3(width / 2.0f, height / 2.0f, 0);
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
        List<char> chars = new List<char>();

        foreach (TMP_CharacterInfo characterInfo in textInfo.characterInfo)
        {
            chars.Add(characterInfo.character);
        }

        string fullString = new string(chars.ToArray());

        int first = fullString.LastIndexOf(toFind);
        int last = first + toFind.Length;
        
        return new Vector2Int(first, last);
    }
    
    //This code is wildly inefficient but I don't think it should matter, it's not called often
    //might be wonky with rich text, this is a first implementation
    #region deletions
    public void DeletePrevious(string startString, string endString)
    {
        if(!output.text.Contains(startString))
        {
            Debug.LogError("Couldn't find string: " + startString);
            return;
        }
        else if(!output.text.Contains(endString))
        {
            Debug.LogError("Couldn't find string: " + endString);
            return;
        }
        else
        {

            string[] splitByEndString = SplitStringWithString(output.text, endString);

            string beforeEndString = "";

            //minus one because we don't want to include the last bit
            for(int i = 0; i < splitByEndString.Length - 1; i++)
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
