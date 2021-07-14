using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;

    public List<string> keyWords;

    // Start is called before the first frame update
    void Start()
    {
        keywordRecognizer = new KeywordRecognizer(keyWords.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += recognizedWord;
        keywordRecognizer.Start();
        if (keywordRecognizer.IsRunning)
        {
            Debug.Log("keyword recognizer is running");
        }
    }

    public void recognizedWord(PhraseRecognizedEventArgs phrase)
    {
        
        Debug.Log(phrase.text);
    }

}
