using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TextMeshProUGUI))]
public class ClarityText : MonoBehaviour
{
    [HideInInspector] //TODO make private, need to work on the abstraction a bit
    public TextMeshProUGUI output;
    private static string highlightPrefix = "<mark=#ccff0033>";//"<mark=#22222233>";
    private static string highlightSuffix = "</mark>";

    public float timeBetweenLetters = 0.03f, variance = 0.01f;
    // Start is called before the first frame update
    void Awake()
    {
        output = GetComponent<TextMeshProUGUI>();
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
                if(newText[i] == '<'){
                    Debug.Log(newText.Length + " " + i  + " " + maxRichTextLength);
                
                }
            
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
    
    public void RemoveHighlights()
    {
        output.text = output.text.Replace(highlightPrefix, "");
        output.text = output.text.Replace(highlightSuffix, "");
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

        string beforeParse = output.text.Substring(0, firstIndex);
        string parse = wordToHypertext;
        string afterParse = output.text.Substring(firstIndex + wordToHypertext.Length);

        string parsed = highlightPrefix + parse + highlightSuffix;

        output.text = beforeParse + parsed + afterParse;
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
