using UnityEngine;
using System.Collections;
using TMPro;


public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textContent;
    string story;
    [SerializeField] float delayBetweenCharacters = 0.125f;

    void Awake()
    {
        story = textContent.text;
        textContent.text = "";

        StartCoroutine("PlayText");
    }

    IEnumerator PlayText()
    {
        while (true)
        {
            foreach (char c in story)
            {
                if (textContent.enabled)
                {
                    textContent.text += c;
                }
                yield return new WaitForSeconds(delayBetweenCharacters);
            }
            yield return new WaitForSeconds(1f);
            if (textContent.enabled)
            {
                textContent.text = "";
            }
        }
    }

}
