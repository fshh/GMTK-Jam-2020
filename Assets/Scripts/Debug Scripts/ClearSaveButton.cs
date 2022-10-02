using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSaveButton : MonoBehaviour
{
    public void ClearSave()
    {
        SaveWrapper.Instance.ClearStory();
    }
}
