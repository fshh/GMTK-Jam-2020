using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESCMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject ButtonPanel;
    public GameObject OptionsPanel;
    public GameObject CreditsPanel;

    [Header("Options")]
    public Toggle CRTToggle;

    private bool isActive = false;

    private static ESCMenu instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            LoadPreferences();
            SwitchToPanel(ButtonPanel);
            UpdateChildrenActive();
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            isActive = !isActive;
            SwitchToPanel(ButtonPanel);
            UpdateChildrenActive();
        }
    }

    private void LoadPreferences()
    {
        CRTToggle.isOn = PlayerPrefs.GetInt("CRTEffect", 1) == 1;
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
        OptionsPanel.SetActive(false);
        CreditsPanel.SetActive(false);

        panel.SetActive(true);
    }

    public void ToButtons()
    {
        SwitchToPanel(ButtonPanel);
    }

    public void Options()
    {
        SwitchToPanel(OptionsPanel);
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

    public void ToggleCRTEffect(bool enabled)
    {
        if (enabled)
        {
            PlayerPrefs.SetInt("CRTEffect", 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("CRTEffect", 0);
            PlayerPrefs.Save();
        }
    }
}
