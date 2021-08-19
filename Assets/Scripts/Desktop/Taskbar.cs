using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taskbar : Singleton<Taskbar>
{
	public RectTransform ButtonParent;
	public GameObject ButtonPrefab;

	[SerializeField]
	private List<ApplicationSO> applications = null;
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

	public TaskbarButton GetButton(ApplicationSO app)
    {
		return buttons.Find(b => b.App == app);
    }

	public void OpenSettings()
	{
		throw new System.NotImplementedException();
	}
}
