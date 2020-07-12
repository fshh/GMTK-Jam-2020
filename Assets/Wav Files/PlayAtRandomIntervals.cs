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

    private float seed;

    private float volumeMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();

        nextTime += Random.Range(minTime, maxTime);

        seed = Random.Range(0, 100);

        volumeMultiplier = sound.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!sound.isPlaying && Time.time > nextTime)
        {
            nextTime += Random.Range(minTime, maxTime);

            sound.clip = sounds[Random.Range(0, sounds.Count)];

            sound.Play();
        }

        sound.panStereo = (Mathf.PerlinNoise(Time.time / 3, seed) * 2) - 1;

        sound.volume = (1 - (sound.panStereo * sound.panStereo)) * volumeMultiplier;
    }
}
