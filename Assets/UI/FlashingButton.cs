using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingButton : MonoBehaviour
{
    float timer;
    [SerializeField] GameObject buttonImage;


    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            buttonImage.SetActive(true);
        }

        if (timer >= 1.5)
        {
            buttonImage.SetActive(false);
            timer = 0;
        }
    }
}
