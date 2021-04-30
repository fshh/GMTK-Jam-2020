using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taskbar : Singleton<Taskbar>
{
	public RectTransform ButtonParent;
	public GameObject ButtonPrefab;

	[SerializeField]
	private List<ApplicationSO> applications;
	private List<TaskbarButton> buttons = new List<TaskbarButton>();

	private void Awake()
	{
		foreach(ApplicationSO app in applications)
		{
			TaskbarButton button = Instantiate(ButtonPrefab, ButtonParent).GetComponent<TaskbarButton>();
			button.Initialize(app);
			buttons.Add(button);
		}
	}

	public void OpenSettings()
	{
		throw new System.NotImplementedException();
	}
}
