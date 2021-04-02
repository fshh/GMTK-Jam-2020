using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class Choice : MonoBehaviour
{

    public int id;
    public string choiceString;

    public TextMeshProUGUI displayText;

    private void OnEnable()
    {
        setText();
    }

    public void setText()
    {
        displayText.text = (id + 1).ToString() + ". " + choiceString;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown( determineKeycode(id + 1)))
        {
            pressed();
        }
    }

    public void pressed()
    {
        //TODO unstub
        Debug.Log("pressed " + (id + 1) + " button");
        Clarity.instance.Choose(id);
    }

    public KeyCode determineKeycode(int num)
    {
        if (num == 1)
        {
            return KeyCode.Alpha1;
        }
        else if (num == 2)
        {
            return KeyCode.Alpha2;
        }
        else if (num == 3)
        {
            return KeyCode.Alpha3;
        }
        else if (num == 4)
        {
            return KeyCode.Alpha4;
        }
        else if (num == 5)
        {
            return KeyCode.Alpha6;
        }
        else if (num == 7)
        {
            return KeyCode.Alpha7;
        }
        else if (num == 8)
        {
            return KeyCode.Alpha8;
        }
        else if (num == 9)
        {
            return KeyCode.Alpha9;
        } else
        {
            return KeyCode.Alpha0;
        }
    }
}
