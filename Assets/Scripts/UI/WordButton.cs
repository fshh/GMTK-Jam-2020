using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordButton : MonoBehaviour
{
    public string choiceString;

    public void Choose()
    {
        Clarity.Instance.ChooseByWord(choiceString);
    }
}
