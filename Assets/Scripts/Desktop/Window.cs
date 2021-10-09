using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Window : MonoBehaviour, IPointerClickHandler
{
	public RectTransform ContentParent;
	public Image AppIcon;
	public TextMeshProUGUI TitleText;
	public Button FullscreenButton;
	public Button UnFullscreenButton;
	public DragTarget WindowDragger;
	public ApplicationSO App { get; private set; }
	public bool Minimized { get => canvasGroup.alpha == 0f; }

	public GameObject content;
	private SaveDataGeneric<GameObject> save;
	private CanvasGroup canvasGroup;

	private class WindowPosAndSize
    {
		public readonly Vector2 AnchoredPos;
		public readonly Vector2 SizeDelta;

		public WindowPosAndSize(Vector2 anchoredPos, Vector2 sizeDelta)
        {
			AnchoredPos = anchoredPos;
			SizeDelta = sizeDelta;
        }
    }
	private WindowPosAndSize prevPosAndSize;

    private void Awake()
    {
		canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnDestroy()
	{
		WindowManager.Instance?.RemoveWindow(this);
	}

	public void Initialize(ApplicationSO app)
	{
		App = app;
		content = Instantiate(app.AppContentPrefab, ContentParent);
		RectTransform contentRect = content.GetComponent<RectTransform>();
		contentRect.pivot = new Vector2(0.5f, 0.5f);
		contentRect.anchorMax = Vector2.one;
		contentRect.anchorMin = Vector2.zero;
		contentRect.anchoredPosition = Vector2.zero;
		contentRect.offsetMax = Vector2.zero;
		contentRect.offsetMin = Vector2.zero;

		AppIcon.sprite = App.Icon;
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
		canvasGroup.alpha = 1f;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		transform.SetAsLastSibling();
		WindowManager.Instance.FocusWindow(this);
	}

	public void Minimize()
	{
		canvasGroup.alpha = 0f;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		transform.SetAsFirstSibling();
		WindowManager.Instance.MinimizeWindow(this);
	}

	public void Close()
	{
		// TODO
		//Save();
		transform.SetAsFirstSibling();
		WindowManager.Instance.RemoveWindow(this);
		Destroy(gameObject);
	}

	public void Fullscreen()
	{
		RectTransform rectTransform = GetComponent<RectTransform>();
		prevPosAndSize = new WindowPosAndSize(rectTransform.anchoredPosition, rectTransform.sizeDelta);
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.anchoredPosition = Vector2.zero;
		rectTransform.sizeDelta = Vector2.zero;
		FullscreenButton.gameObject.SetActive(false);
		UnFullscreenButton.gameObject.SetActive(true);
		WindowDragger.gameObject.SetActive(false);
	}

	public void UnFullscreen()
	{
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		rectTransform.anchoredPosition = prevPosAndSize.AnchoredPos;
		rectTransform.sizeDelta = prevPosAndSize.SizeDelta;
		FullscreenButton.gameObject.SetActive(true);
		UnFullscreenButton.gameObject.SetActive(false);
		WindowDragger.gameObject.SetActive(true);
	}

	public void Save()
	{
		throw new System.NotImplementedException();
	}
}
