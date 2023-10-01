using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraSwap2D3D : MonoBehaviour
{
    public Camera screenCamera;
    public RenderTexture screenOutputTexture;
    public Transform camera3D;
    public Transform target2D;
    public Transform target3D;
    public float transitionTime;
    public CanvasGroup uiCanvasGroup;
    public MouseCursor mouseCursor;
    public Camera1stPerson camera1stPerson;

    private bool in2D = true;
    private bool transitioning = false;

    private void Awake()
    {
        Settings2D();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !transitioning)
        {
            if (in2D)
            {
                Sequence to3DSequence = DOTween.Sequence()
                    .Append(camera3D.DOMove(target3D.position, transitionTime))
                    .Join(camera3D.DORotateQuaternion(target3D.rotation, transitionTime))
                    .SetEase(Ease.OutCubic)
                    .OnStart(() => { transitioning = true; Settings3D(); })
                    .OnComplete(() => { transitioning = false; camera1stPerson.enabled = true; });
                to3DSequence.Play();
            }
            else
            {
                Sequence to2DSequence = DOTween.Sequence()
                    .Append(camera3D.DOMove(target2D.position, transitionTime))
                    .Join(camera3D.DORotateQuaternion(target2D.rotation, transitionTime))
                    .SetEase(Ease.InCubic)
                    .OnStart(() => { transitioning = true; camera1stPerson.enabled = false; })
                    .OnComplete(() => { transitioning = false; Settings2D(); });
                to2DSequence.Play();
            }
        }
    }

    private void Settings2D()
    {
        screenCamera.targetTexture = null;
        screenCamera.depth = 1;
        in2D = true;
        uiCanvasGroup.interactable = true;
        mouseCursor.enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Settings3D()
    {
        screenCamera.targetTexture = screenOutputTexture;
        screenCamera.depth = -1;
        in2D = false;
        uiCanvasGroup.interactable = false;
        mouseCursor.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
