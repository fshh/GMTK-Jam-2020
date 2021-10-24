using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TempDebugScript : MonoBehaviour
{

    private bool mute;
    public bool Mute
    {
        set { masterMixer.SetFloat("MasterVolume", value ? -80 : 0); }
    }

    public AudioMixer masterMixer;
    
    public void SkipWaiting(bool value)
    {
        Clarity clarity = (Clarity)FindObjectOfType(typeof(Clarity));
        if (clarity != null)
        {
            clarity.NotWaiting = value;
        }
        
    }
}
