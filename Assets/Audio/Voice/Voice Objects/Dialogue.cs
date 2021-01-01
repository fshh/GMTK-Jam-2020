using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VA", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public AudioClip audio;
    public string subtitle;
}
