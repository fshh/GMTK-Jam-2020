using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class VaryVolumeSin : MonoBehaviour
{
    private AudioSource music;

    private float maxVolume;

    public float period;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
        maxVolume = music.volume;
    }

    // Update is called once per frame
    void Update()
    {
        music.volume = ( Mathf.Sin(2 * Mathf.PI * Time.time / period) + 1) * maxVolume / 2;
    }
}
