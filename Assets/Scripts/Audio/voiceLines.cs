using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class voiceLines : MonoBehaviour
{
    public List<AudioClip> lines;

    AudioSource player;

    int currClip = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
    }

    public float playNextLine()
    {
        player.clip = lines[currClip++];
        player.Play();
        return player.clip.length;
    }
}
