using System.Collections;
using UnityEngine;

public class BlinkingCursor : MonoBehaviour
{

    private float m_TimeStamp;
    private bool cursor = false;
    private string cursorChar = "";

    void update()
    {
        if (Time.time - m_TimeStamp >= 0.5)
        {
            m_TimeStamp = Time.time;
            if (cursor == false)
            {
                cursor = true;
                cursorChar += "_";
            }
            else
            {
                cursor = false;
                if (cursorChar.Length != 0)
                {
                    cursorChar = cursorChar.Substring(0, cursorChar.Length - 1);
                }
            }
        }
    }
}
