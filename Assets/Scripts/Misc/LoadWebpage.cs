using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWebpage : MonoBehaviour
{
    public string baseURL;

    [System.Obsolete]
    public void OpenPage(string s)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Application.ExternalEval("window.open(\"" + s + "\",\"_blank\")");
            return;
        } else
        {
            Application.OpenURL(s);
        }
    }
}
