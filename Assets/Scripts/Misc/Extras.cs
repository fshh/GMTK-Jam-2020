using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extras : MonoBehaviour
{
    public static T RandomElement<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    
    public static T RandomElement<T>(T[] list)
    {
        return list[Random.Range(0, list.Length)];
    }
}
