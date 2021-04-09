using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnKey : MonoBehaviour
{
    public KeyCode key = KeyCode.Return;

    public int sceneToLoad = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            loadScene();
        }
    }

    public void loadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
