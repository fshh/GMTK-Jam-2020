using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashingText : MonoBehaviour
{
    float timer;
    [SerializeField] TextMeshProUGUI textContent;

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
