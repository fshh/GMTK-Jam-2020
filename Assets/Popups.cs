using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popups : MonoBehaviour
{
    public Image displayed;

    public Animator anim;

    public Sprite clipboard;
    public Sprite copy;

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftControl)) && (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.X)) )
        {
            displayed.sprite = copy;
            anim.SetTrigger("Popup");
        }

        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.V))
        {
            displayed.sprite = clipboard;
            anim.SetTrigger("Popup");
        }
    }
}
