using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour, IPointerClickHandler
{
	public RectTransform ContentParent;
	public TextMeshProUGUI TitleText;
	public ApplicationSO App { get; private set; }

	private GameObject content;
	private SaveDataGeneric<GameObject> save;

	private void OnDestroy()
	{
		WindowManager.Instance.RemoveWindow(this);
	}

	public void Initialize(ApplicationSO app)
	{
		App = app;
		content = Instantiate(app.AppContentPrefab, ContentParent);
		RectTransform rect = content.GetComponent<RectTransform>();
		rect.pivot = new Vector2(0.5f, 0.5f);
		rect.anchorMax = Vector2.one;
		rect.anchorMin = Vector2.zero;
		rect.anchoredPosition = Vector2.zero;
		rect.offsetMax = Vector2.zero;
		rect.offsetMin = Vector2.zero;

		gameObject.name = App.Name + "Window";
		TitleText.text = App.Name;

		WindowManager.Instance.AddWindow(this);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Focus();
	}

	public void Focus()
	{
		WindowManager.Instance.FocusWindow(this);
	}

	public void Close()
	{
		throw new System.NotImplementedException();
	}

	public void Minimize()
	{
		throw new System.NotImplementedException();
	}

	public void Fullscreen()
	{
		throw new System.NotImplementedException();
	}

	public void UnFullscreen()
	{
		throw new System.NotImplementedException();
	}

	public void Save()
	{
		throw new System.NotImplementedException();
	}
}
