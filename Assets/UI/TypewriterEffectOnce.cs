using UnityEngine;
using System.Collections;
using TMPro;


public class TypewriterEffectOnce : MonoBehaviour
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
            foreach (char c in story)
            {
                textContent.text += c;
                yield return new WaitForSeconds(delayBetweenCharacters);
            }
    }

}
