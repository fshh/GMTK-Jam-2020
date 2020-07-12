using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCMenu : MonoBehaviour
{
    public GameObject ButtonPanel;
    public GameObject CreditsPanel;

    private bool isActive = false;

    private void Awake()
    {
        SwitchToPanel(ButtonPanel);
        UpdateChildrenActive();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            isActive = !isActive;
            UpdateChildrenActive();
        }
    }

    private void UpdateChildrenActive()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }

    private void SwitchToPanel(GameObject panel)
    {
        ButtonPanel.SetActive(false);
        CreditsPanel.SetActive(false);

        panel.SetActive(true);
    }

    public void ToButtons()
    {
        SwitchToPanel(ButtonPanel);
    }

    public void Credits()
    {
        SwitchToPanel(CreditsPanel);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
