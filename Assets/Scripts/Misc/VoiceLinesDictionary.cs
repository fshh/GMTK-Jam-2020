using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class VoiceLinesDictionary : MonoBehaviour
{
    [Header("Subtitles Objects")]
    public TextMeshProUGUI subtitles;
    public Image subtitleBackground;

    [Header("Lines of Dialogue")]
    public List<Dialogue> lines;
    public Dictionary<string, Dialogue> linesDictionary;

    AudioSource player;

    int currClip = 0;

    // Start is called before the first frame update
    void Start()
    {
        linesDictionary = new Dictionary<string, Dialogue>();
        foreach(Dialogue nugget in lines)
        {
            linesDictionary.Add(nugget.name, nugget);
        }

        player = GetComponent<AudioSource>();
    }

    public void playLine(string lineName)
    {
        if (linesDictionary.ContainsKey(lineName))
        {
            player.clip = linesDictionary[lineName].audio;
            player.Play();

            StartCoroutine(setSubtitles(linesDictionary[lineName].subtitle, player.clip.length + 1f));
        }
        else
        {
            Debug.Log("No such voiceover exists: " + lineName);
        }
    }

    IEnumerator setSubtitles(string subtitle, float duration)
    {
        subtitles.text = subtitle;
        subtitleBackground.enabled = true;
        yield return new WaitForSeconds(duration);
        subtitles.text = "";
        subtitleBackground.enabled = false;
    }

}
