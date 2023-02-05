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
        if (WindowManager.Instance.NumWindowsOpen(App) > 1)
        {
            bool anyMinimized = false;
            foreach (Window window in WindowManager.Instance.GetWindows(App))
            {
                anyMinimized |= window.Minimized;
            }

            if (anyMinimized || !WindowManager.Instance.HasWindowFocused(App))
            {
                Window finalFocusedWindow = null;
                if (WindowManager.Instance.HasWindowFocused(App))
                {
                    finalFocusedWindow = WindowManager.Instance.GetFocusedWindow();
                }

                FocusWindows(finalFocusedWindow);
            }
            else
            {
                MinimizeWindows();
            }
        }
        else if (WindowManager.Instance.NumWindowsOpen(App) > 0)
        {
            if (WindowManager.Instance.HasWindowFocused(App))
            {
                MinimizeWindows();
            }
            else
            {
                FocusWindows();
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

    private void FocusWindows(Window finalFocusedWindow = null)
    {
        foreach (Window window in WindowManager.Instance.GetWindows(App))
        {
            window.Focus();
        }

        if (finalFocusedWindow != null)
        {
            finalFocusedWindow.Focus();
        }
    }

    private void MinimizeWindows()
    {
        foreach (Window window in WindowManager.Instance.GetWindows(App))
        {
            window.Minimize();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
}
