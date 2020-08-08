using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashingText : MonoBehaviour
{
    public TextMeshProUGUI textContent;
    private float timer;

    void Update()
    {
        flash();
    }

    public void flash()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            textContent.enabled = true;
        }

        if (timer >= 1.5)
        {
            textContent.enabled = false;
            timer = 0;
        }
    }
}
