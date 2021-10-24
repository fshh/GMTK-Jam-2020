using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskbarButton : MonoBehaviour, IPointerClickHandler
{
    public Image AppIcon;
    public Image HasWindowOpenIndicator;
    public Color NoWindowOpenColor;
    public Color HasWindowOpenColor;

    public ApplicationSO App { get; private set; }

    private PointerEventColorSettings colorSettings;

    public void Initialize(ApplicationSO app)
    {
        App = app;
        AppIcon.sprite = App.TaskbarIcon;
        gameObject.name = App.Name + "TaskbarButton";
        colorSettings = GetComponent<PointerEventColorSettings>();
    }

    private void Update()
    {
        colorSettings?.SetSelected(WindowManager.Instance.HasWindowFocused(App));
        HasWindowOpenIndicator.color = WindowManager.Instance.HasWindowsOpen(App) ? HasWindowOpenColor : NoWindowOpenColor;
    }

    public void OnClick()
    {
        Debug.Log("hello");
        if (WindowManager.Instance.NumWindowsOpen(App) > 1)
        {
            WindowSelection();
        }
        else if (WindowManager.Instance.NumWindowsOpen(App) > 0)
        {
            if (WindowManager.Instance.HasWindowFocused(App))
            {
                MinimizeWindow();
            }
            else
            {
                FocusWindow();
            }
        }
        else
        {
            OpenWindow();
        }
    }

    public void SelectWindow(Window window)
    {
        window.Focus();
    }

    private void OpenWindow()
    {
        App.OpenWindow();
    }

    private void WindowSelection()
    {
        throw new System.NotImplementedException();
    }

    private void FocusWindow()
    {
        WindowManager.Instance.GetWindows(App)[0].Focus();
    }

    private void MinimizeWindow()
    {
        WindowManager.Instance.GetWindows(App)[0].Minimize();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
}
