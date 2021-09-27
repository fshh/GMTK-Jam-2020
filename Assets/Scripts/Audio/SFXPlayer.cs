using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    public List<AudioClip> clips;

    private AudioSource[] audioSources;


    // Start is called before the first frame update
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void PlayRandom()
    {
        if (!audioSources[0].isPlaying)
        {            
            if (Random.Range(0, 2) > 0)
            {
                audioSources[0].clip = Extras.RandomElement(clips);
                audioSources[0].time = Random.Range(audioSources[0].clip.length / 2f, audioSources[0].clip.length);
                //audioSources[0].pitch = Random.Range(1.5f, 1.6f);
            
                audioSources[0].Play();
                //audioSources[0].PlayOneShot(Extras.RandomElement(clips));
            }
            else
            {
                audioSources[0].clip = Extras.RandomElement(clips);
                audioSources[0].Play();
            }
        }
    }
}
