using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingObject : MonoBehaviour
{
    public GameObject Object;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            Object.SetActive(true);
        }

        if (timer >= 1.5)
        {
            Object.SetActive(false);
            timer = 0;
        }
    }
}
