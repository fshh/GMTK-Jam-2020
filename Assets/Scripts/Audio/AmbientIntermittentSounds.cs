using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientIntermittentSounds : MonoBehaviour
{
    public List<AudioClip> sounds;
    public float minTime, maxTime;
    public float minVolumeMultiplier, maxVolumeMultiplier;

    private List<AudioSource> sources;

    // Start is called before the first frame update
    void Awake()
    {
        sources = new List<AudioSource>(GetComponentsInChildren<AudioSource>());
        StartCoroutine(PlayIntermittentSounds());
    }

    private IEnumerator PlayIntermittentSounds()
    {
        while (true)
        {
            float timeUntilNext = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(timeUntilNext);
            AudioSource source = sources[Random.Range(0, sources.Count)];
            source.PlayOneShot(sounds[Random.Range(0, sounds.Count)], Random.Range(minVolumeMultiplier, maxVolumeMultiplier));
        }
    }
}
