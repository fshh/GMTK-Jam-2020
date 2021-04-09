using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWebpage : MonoBehaviour
{

    public void Load(string pageName)
    {
        Application.OpenURL(pageName);
    }
}
