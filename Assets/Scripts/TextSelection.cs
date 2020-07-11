using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextSelection : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Highlighting
    [Header("Highlighting")]
    public Transform HighlightsParent;
    public GameObject HighlightPrefab;
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

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textMesh = GetComponent<TextMeshProUGUI>();
        canvas = GetComponentInParent<Canvas>();

        PlayerInput.SubmitTextEvent += OnSubmitText;

        ResetSelectionIndices();
    }

    private void Update()
    {
        if (isHolding && TMP_TextUtilities.IsIntersectingRectTransform(rectTransform, Input.mousePosition, Camera.main))
        {
            int wordIndex = TMP_TextUtilities.FindIntersectingWord(textMesh, Input.mousePosition, Camera.main);
            if (wordIndex >= 0 && wordIndex != selectionEndIndex)
            {
                if (selectionStartIndex < 0) { selectionStartIndex = wordIndex; }
                selectionEndIndex = wordIndex;

                SelectWords();
            }
        }
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

            ResetSelectionIndices();
            ResetHighlights();
            ResetSelectedText();
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
        GameObject obj = Instantiate(HighlightPrefab, HighlightsParent);
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
        foreach (Transform child in HighlightsParent) { Destroy(child.gameObject); }
        highlights.Clear();
    }

    private void ResetSelectedText()
    {
        SelectedText = "";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ResetSelectionIndices();
        ResetHighlights();
        ResetSelectedText();
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    private void OnSubmitText(string text)
    {
        if (SelectedInstance == this)
        {
            SelectedInstance = null;
            ResetSelectionIndices();
            ResetHighlights();
            ResetSelectedText();
        }
    }
}
