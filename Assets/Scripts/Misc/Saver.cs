using System.Collections;
using System.Collections.Generic;
using CI.QuickSave;
using UnityEngine;

public class Saver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO test if this works and flesh it out more
    public void SaveInkState()
    { 
        QuickSaveWriter.Create("StoryState")
            .Write("State", HyperInkWrapper.instance.GetStateJson())
            .Commit();
        
    }
}
