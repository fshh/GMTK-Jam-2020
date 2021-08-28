using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDebugScript : MonoBehaviour
{
    public void SkipWaiting(bool value)
    {
        Clarity clarity = (Clarity)FindObjectOfType(typeof(Clarity));
        if (clarity != null)
        {
            clarity.NotWaiting = value;
        }
        
    }
    
}
