using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap2D3D : MonoBehaviour
{
    public Camera screenCamera;
    public RenderTexture screenOutputTexture;

    private bool in2D = true;

    private void Awake()
    {
        SwapTo2D();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (in2D)
            {
                SwapTo3D();
            }
            else
            {
                SwapTo2D();
            }
        }
    }

    private void SwapTo2D()
    {
        screenCamera.targetTexture = null;
        screenCamera.depth = 1;
        in2D = true;
    }

    private void SwapTo3D()
    {
        screenCamera.targetTexture = screenOutputTexture;
        screenCamera.depth = -1;
        in2D = false;
    }
}
