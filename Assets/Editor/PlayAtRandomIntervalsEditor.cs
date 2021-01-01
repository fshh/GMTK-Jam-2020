using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayAtRandomIntervals))]
public class PlayAtRandomIntervalsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayAtRandomIntervals myScript = (PlayAtRandomIntervals)target;
        if (GUILayout.Button("Play Sound"))
        {
            myScript.PlaySound();
        }
    }
}
