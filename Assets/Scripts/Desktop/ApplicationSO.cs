using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Application", menuName = "Application", order = 1)]
public class ApplicationSO : ScriptableObject
{
	public string Name;
	public Sprite Icon;
	public GameObject AppContentPrefab;

	private static GameObject WindowPrefab = null;

	public Window OpenWindow()
	{
		if (!WindowPrefab)
		{
			WindowPrefab = Resources.Load<GameObject>("Window");
		}

		Window window = Instantiate(WindowPrefab).GetComponent<Window>();
		window.Initialize(this);
		return window;
	}

	public Window OpenWindow(string fileName)
	{
		// TODO: handle apps that open files?
		return OpenWindow();
	}

	public void Save()
	{
		throw new System.NotImplementedException();
	}

	public void Quit()
	{
		throw new System.NotImplementedException();
	}
}
