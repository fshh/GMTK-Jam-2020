using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextSelection : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Receive Player Submissions?")]
    public bool receiveSubmissions = false;

    // Highlighting
    private Transform highlightsParent;
    private GameObject highlightPrefab;
    private static float paddingX = 5f;
    private static float paddingY = 5f;
    private struct Highlight
    {
        public Highlight(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }
        public Vector2 position;
        public Vector2 size;
    }
    private static List<Highlight> highlights = new List<Highlight>();

    // Text selection
    private bool isHolding;
    private int selectionStartIndex;
    private int selectionEndIndex;
    public static string SelectedText { get; private set; }
    public static TextSelection SelectedInstance { get; private set; }

    // Components
    private RectTransform rectTransform;
    private TextMeshProUGUI textMesh;
    private Canvas canvas;

    bool useCurve;
    private float curveX;
    private float curveY;
    private float zoom;

    private void Awake()
    {
        highlightsParent = GameObject.FindGameObjectWithTag("HighlightsParent").transform;
        highlightPrefab = (GameObject)Resources.Load("Highlight");

        rectTransform = GetComponent<RectTransform>();
        textMesh = GetComponent<TextMeshProUGUI>();
        canvas = GetComponentInParent<Canvas>();

        Material screenEffect = (Material)Resources.Load("ScreenEffect");
        useCurve = screenEffect.GetFloat("_UseCurve") == 1f;
        curveX = screenEffect.GetFloat("_CurveX");
        curveY = screenEffect.GetFloat("_CurveY");
        zoom = screenEffect.GetFloat("_Zoom");

        PlayerInput.SubmitTextEvent += OnSubmitText;

        ResetSelectionIndices();
    }

    private void Update()
    {
        if (isHolding)
        {
            //Vector3 mousePos = ScreenCurvePoint(Input.mousePosition);
            Vector3 mousePos = Input.mousePosition;
            if (TMP_TextUtilities.IsIntersectingRectTransform(rectTransform, mousePos, Camera.main))
            {
                int wordIndex = TMP_TextUtilities.FindIntersectingWord(textMesh, mousePos, Camera.main);
                if (wordIndex >= 0 && wordIndex != selectionEndIndex)
                {
                    if (selectionStartIndex < 0) { selectionStartIndex = wordIndex; }
                    selectionEndIndex = wordIndex;

                    SelectWords();
                }
            }
        }
    }

    // Adjust screen-space coordinate according to screen effect's curve function
    // Not needed because custom cursor is also affected by screen effect
    private Vector3 ScreenCurvePoint(Vector3 screenPoint)
    {
        if (!useCurve) { return screenPoint; }

        Vector2 uv = new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
        uv = (uv - new Vector2(0.5f, 0.5f)) * 2.0f;
        uv *= 1.1f;
        uv.x *= 1.0f + Mathf.Pow((Mathf.Abs(uv.y) / curveX), 2);
        uv.y *= 1.0f + Mathf.Pow((Mathf.Abs(uv.x) / curveY), 2);
        uv *= zoom;
        uv = (uv / 2.0f) + new Vector2(0.5f, 0.5f);
        uv = uv * 0.92f + new Vector2(0.04f, 0.04f);

        return new Vector3(uv.x * Screen.width, uv.y * Screen.height, screenPoint.z);
    }

    public void DeleteSelection()
    {
        if (SelectedInstance == this)
        {
            int firstWord = Math.Min(selectionStartIndex, selectionEndIndex);
            int lastWord = Math.Max(selectionStartIndex, selectionEndIndex);

            int firstChar = textMesh.textInfo.wordInfo[firstWord].firstCharacterIndex;
            int lastChar = textMesh.textInfo.wordInfo[lastWord].lastCharacterIndex;

            string text = textMesh.text;
            string removed = text.Remove(firstChar, lastChar - firstChar + 2);
            textMesh.text = removed;

            FullReset();
        }
    }

    private void SelectWords()
    {
        SelectedInstance = this;

        // Reset highlights
        ResetHighlights();

        // Reset selected text
        ResetSelectedText();

        int firstWord = Math.Min(selectionStartIndex, selectionEndIndex);
        int lastWord = Math.Max(selectionStartIndex, selectionEndIndex);

        int firstChar = textMesh.textInfo.wordInfo[firstWord].firstCharacterIndex;
        int lastChar = textMesh.textInfo.wordInfo[lastWord].lastCharacterIndex;

        bool isBeginRegion = false;

        Vector3 bottomLeft = Vector3.zero;
        Vector3 topLeft = Vector3.zero;
        Vector3 bottomRight = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        float maxAscender = -Mathf.Infinity;
        float minDescender = Mathf.Infinity;

        for (int characterIndex = firstChar; characterIndex <= lastChar; characterIndex++)
        {
            TMP_CharacterInfo currentCharInfo = textMesh.textInfo.characterInfo[characterIndex];
            int currentLine = currentCharInfo.lineNumber;

            bool isCharacterVisible = characterIndex > textMesh.maxVisibleCharacters ||
                currentCharInfo.lineNumber > textMesh.maxVisibleLines ||
                (textMesh.overflowMode == TextOverflowModes.Page && currentCharInfo.pageNumber + 1 != textMesh.pageToDisplay) ? false : true;

            // Track Max Ascender and Min Descender
            maxAscender = Mathf.Max(maxAscender, currentCharInfo.ascender);
            minDescender = Mathf.Min(minDescender, currentCharInfo.descender);

            // Save character to selected text
            char character = currentCharInfo.character;
            if (isCharacterVisible && character != '\n') { SelectedText += character; }

            // Start of selected region
            if (isBeginRegion == false && isCharacterVisible)
            {
                isBeginRegion = true;

                bottomLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.descender, 0);
                topLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.ascender, 0);

                //Debug.Log("Start Region at [" + currentCharInfo.character + "]");
            }

            // Last Character of Selection
            if (isBeginRegion && characterIndex == lastChar)
            {
                isBeginRegion = false;

                Vector2 size = new Vector2(currentCharInfo.bottomRight.x - bottomLeft.x, maxAscender - minDescender);

                topLeft = rectTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                bottomLeft = rectTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                bottomRight = rectTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                topRight = rectTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                Vector2 position = new Vector2((bottomRight.x + bottomLeft.x) / 2f, (topRight.y + bottomRight.y) / 2f);

                // Save highlight
                highlights.Add(new Highlight(position, size));

                //Debug.Log("End Region at [" + currentCharInfo.character + "] (last character)");
            }
            // If Selection is split on more than one line.
            else if (isBeginRegion && currentLine != textMesh.textInfo.characterInfo[characterIndex + 1].lineNumber)
            {
                isBeginRegion = false;

                Vector2 size = new Vector2(currentCharInfo.bottomRight.x - bottomLeft.x, maxAscender - minDescender);

                topLeft = rectTransform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                bottomLeft = rectTransform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                bottomRight = rectTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                topRight = rectTransform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                Vector2 position = new Vector2((bottomRight.x + bottomLeft.x) / 2f, (topRight.y + bottomRight.y) / 2f);

                // Save highlight
                highlights.Add(new Highlight(position, size));

                //Debug.Log("End Region at [" + currentCharInfo.character + "] (end of line)");

                maxAscender = -Mathf.Infinity;
                minDescender = Mathf.Infinity;
            }
        }

        foreach (Highlight highlight in highlights)
        {
            CreateHighlight(highlight);
        }
    }

    private void CreateHighlight(Highlight highlight)
    {
        GameObject obj = Instantiate(highlightPrefab, highlightsParent);
        RectTransform rect = obj.GetComponent<RectTransform>();

        rect.SetPositionAndRotation(highlight.position, Quaternion.identity);

        highlight.size.x += 2f * paddingX;
        highlight.size.y += 2f * paddingY;

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, highlight.size.x);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, highlight.size.y);
    }

    private void ResetSelectionIndices()
    {
        selectionStartIndex = -1;
        selectionEndIndex = -1;
    }

    private void ResetHighlights()
    {
        foreach (Transform child in highlightsParent) { Destroy(child.gameObject); }
        highlights.Clear();
    }

    private void ResetSelectedText()
    {
        SelectedText = "";
    }

    public void FullReset()
    {
        SelectedInstance = null;
        ResetSelectionIndices();
        ResetHighlights();
        ResetSelectedText();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        FullReset();
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    // DO NOT DELETE!!
    // If this isn't here, then OnPointerUp gets called as soon as the mouse moves
    public void OnDrag(PointerEventData eventData) {}

    private void OnSubmitText(string text)
    {
        if (SelectedInstance == this)
        {
            FullReset();
        }

        if (receiveSubmissions)
        {
            Color color = PlayerInput.PlayerTextColor;
            string hexCode = ColorUtility.ToHtmlStringRGBA(color);

            text = $"\n\n<#{hexCode}>> " + text + "</color>";

            textMesh.text += text;
        }
    }
}
