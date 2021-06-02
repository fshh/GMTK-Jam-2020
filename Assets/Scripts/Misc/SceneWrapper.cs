using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneWrapper : MonoBehaviour
{
    public void LoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
