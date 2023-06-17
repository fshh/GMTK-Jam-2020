﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rain_window : MonoBehaviour
{
    public int imgWidth = 64;
    public int imgHeight = 64;
    
    Texture2D texture;
    
    // Start is called before the first frame update
    void Start()
    {
        //SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        texture = new Texture2D(64, 64);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = texture;
        texture.SetPixel(Random.Range(0, imgWidth - 1), Random.Range(0, imgHeight - 1), new Color (0,0,0,0));
        texture.Apply();
        renderer.material.SetTexture("_MainTex", texture);
        //spriteRenderer.sprite = Sprite.Create(texture, new Rect (0, 0, imgWidth, imgHeight), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        texture.SetPixel(Random.Range(0, imgWidth - 1), Random.Range(0, imgHeight - 1), new Color (0,0,0,0));
        texture.Apply();
        GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
    }
}
