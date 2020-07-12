using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    Vector2 cursorPosition;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite loadingCursor;
    [SerializeField] Sprite defaultCursor;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPosition;
    }

    public void SwitchToLoadingCursor()
    {
        spriteRenderer.sprite = loadingCursor;
    }

    public void SwitchToDefaultCursor()
    {
        spriteRenderer.sprite = defaultCursor;
    }
}
