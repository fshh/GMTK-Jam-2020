using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ClaritySpeechSounds : MonoBehaviour
{
    public List<AudioClip> clips;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [Button]
    public void PlayRandom(bool interrupt = false, float truncateAfter = -1f)
    {
        if (interrupt && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(Extras.RandomElement(clips));
        StartCoroutine(TruncateAfter(truncateAfter));
    }

    private IEnumerator TruncateAfter(float seconds)
    {
        if (seconds < 0f)
        {
            yield break;
        }
        yield return new WaitForSecondsRealtime(seconds);
        audioSource.Stop();
    }
}

//Code for making it weird and backwards sometimes
/*if (!audioSource.isPlaying)
{
    if (Random.Range(0, 2) > 0)
    {
        audioSource.clip = Extras.RandomElement(clips);
        //audioSource.time = Random.Range(audioSource.clip.length / 2f, audioSource.clip.length);
        //audioSource.pitch = Random.Range(1.5f, 1.6f);

        audioSource.Play();
        //audioSource.PlayOneShot(Extras.RandomElement(clips));
    }
    else
    {
        audioSource.clip = Extras.RandomElement(clips);
        audioSource.Play();
    }
}*/
