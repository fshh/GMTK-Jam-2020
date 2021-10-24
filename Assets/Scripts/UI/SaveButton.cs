using System.Collections;
using System.Collections.Generic;
using SaveManagement;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public static string INK_STATE = "InkState";
    public static string CLARITY_TEXT = "ClarityText";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveInkState()
    {
        SaveManager.SetData(CLARITY_TEXT, Clarity.Instance.gameObject);
        SaveManager.SetData(INK_STATE, HyperInkWrapper.instance.GetStateJson());
    }

    public void GetInkState()
    {
        HyperInkWrapper.instance.SetState((string)SaveManager.GetData(INK_STATE));
    }
}
