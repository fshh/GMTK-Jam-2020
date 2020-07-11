using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAtRandomIntervals : MonoBehaviour
{
    public List<AudioClip> sounds;

    public float minTime, maxTime;

    float nextTime = 0;

    private AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();

        nextTime += Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(!sound.isPlaying && Time.time > nextTime)
        {
            nextTime += Random.Range(minTime, maxTime);

            sound.clip = sounds[Random.Range(0, sounds.Count)];

            sound.Play();
        }
    }
}
