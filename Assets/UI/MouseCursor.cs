using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    Vector2 cursorPosition;

    private void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPosition;
    }
}
