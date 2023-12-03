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
    private bool inStartMenu = true;

    private void Awake()
    {
        Settings3D();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !transitioning && !inStartMenu && !ClarityText.endgame)
        {
            if (in2D)
            {
                TransitionTo3D(transitionTime);
            }
            else
            {
                TransitionTo2D(transitionTime);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && inStartMenu)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        TransitionTo2D(2.0f);
        inStartMenu = false;
    }

    private void TransitionTo2D(float duration)
    {
        Sequence to2DSequence = DOTween.Sequence()
            .Append(camera3D.DOMove(target2D.position, duration))
            .Join(camera3D.DORotateQuaternion(target2D.rotation, duration))
            .SetEase(Ease.InCubic)
            .OnStart(() => { transitioning = true; camera1stPerson.enabled = false; })
            .OnComplete(() => { transitioning = false; Settings2D(); });
        to2DSequence.Play();
    }

    private void TransitionTo3D(float duration)
    {
        Sequence to3DSequence = DOTween.Sequence()
            .Append(camera3D.DOMove(target3D.position, duration))
            .Join(camera3D.DORotateQuaternion(target3D.rotation, duration))
            .SetEase(Ease.OutCubic)
            .OnStart(() => { transitioning = true; Settings3D(); })
            .OnComplete(() => { transitioning = false; camera1stPerson.enabled = true; });
        to3DSequence.Play();
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
