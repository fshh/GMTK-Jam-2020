using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    Vector2 cursorPosition;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite loadingCursor;
    [SerializeField] Sprite defaultCursor;
    [SerializeField] Sprite smallCursor;
    [SerializeField] AudioClip clickingSound;

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
        if (Input.GetMouseButtonDown(0))
        {
            SwitchToSmallCursor();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SwitchToDefaultCursor();
        }
    }

    private void OnMouseDown()
    {
        if(clickingSound != null)
        {
            AudioSource.PlayClipAtPoint(clickingSound, Camera.main.transform.position);
        }
    }

    public void SwitchToLoadingCursor()
    {
        spriteRenderer.sprite = loadingCursor;
    }

    public void SwitchToDefaultCursor()
    {
        spriteRenderer.sprite = defaultCursor;
    }

    public void SwitchToSmallCursor()
    {
        spriteRenderer.sprite = smallCursor;
    }
}
