using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowManager : Singleton<WindowManager>
{
	private Dictionary<ApplicationSO, List<Window>> openWindowsGrouped = new Dictionary<ApplicationSO, List<Window>>();
	private List<Window> openWindowsAll = new List<Window>();
	private Window focusedWindow;

	public List<Window> GetWindows()
	{
		return openWindowsAll;
	}

	public List<Window> GetWindows(ApplicationSO app)
	{
		return openWindowsGrouped[app];
	}

	public bool HasWindowsOpen(ApplicationSO app)
	{
		return NumWindowsOpen(app) > 0;
	}

	public int NumWindowsOpen(ApplicationSO app)
	{
		if (openWindowsGrouped.ContainsKey(app))
		{
			return openWindowsGrouped[app].Count;
		}

		return 0;
	}

	public bool HasWindowFocused(ApplicationSO app)
	{
		return focusedWindow != null && focusedWindow.App == app;
	}

	public Window GetFocusedWindow()
	{
		return focusedWindow;
	}

	public void AddWindow(Window window)
	{
		if (!openWindowsGrouped.ContainsKey(window.App))
		{
			openWindowsGrouped.Add(window.App, new List<Window>());
		}

		if (!openWindowsGrouped[window.App].Contains(window))
		{
			openWindowsGrouped[window.App].Add(window);
		}

		if(!openWindowsAll.Contains(window))
		{
			openWindowsAll.Add(window);
		}

		window.transform.SetParent(transform);
		window.transform.localScale = Vector3.one;

		RectTransform windowRect = window.GetComponent<RectTransform>();
		windowRect.anchoredPosition = windowRect.anchoredPosition + (openWindowsAll.Count * new Vector2(10f, -10f));

		window.Focus();
	}

	public void RemoveWindow(Window window)
	{
		if (focusedWindow == window)
		{
			FocusNextWindow();
		}

		if (openWindowsGrouped.ContainsKey(window.App) && openWindowsGrouped[window.App].Contains(window))
		{
			openWindowsGrouped[window.App].Remove(window);
		}

		if (openWindowsAll.Contains(window))
		{
			openWindowsAll.Remove(window);
		}
	}

	public void FocusWindow(Window window)
	{
		Taskbar.Instance.GetButton(window.App).GetComponent<PointerEventColorSettings>().SetSelected(true);
		focusedWindow = window;
	}

	public void MinimizeWindow(Window window)
    {
		if (focusedWindow == window)
        {
			FocusNextWindow();
        }
    }

	private void FocusNextWindow()
    {
		Window w = transform.GetChild(transform.childCount - 1).GetComponent<Window>();
		if (!w.Minimized)
		{
			w.Focus();
		}
		else
		{
			focusedWindow = null;
		}
	}
}
