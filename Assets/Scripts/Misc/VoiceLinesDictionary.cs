using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class labeledAudio
{
    public string label;
    public AudioClip audio;
}

[RequireComponent(typeof(AudioSource))]
public class VoiceLinesDictionary : MonoBehaviour
{
    public List<labeledAudio> lines;
    public Dictionary<string, AudioClip> linesDictionary;

    AudioSource player;

    int currClip = 0;

    // Start is called before the first frame update
    void Start()
    {
        linesDictionary = new Dictionary<string, AudioClip>();
        foreach(labeledAudio nugget in lines)
        {
            linesDictionary.Add(nugget.label, nugget.audio);
        }

        player = GetComponent<AudioSource>();
    }

    public float playLine(string lineName)
    {
        if (linesDictionary.ContainsKey(lineName))
        {
            player.clip = linesDictionary[lineName];
            player.Play();
            return player.clip.length;
        }
        else
        {
            Debug.Log("No such voiceover exists: " + lineName);
            return 0;
        }
    }
}
