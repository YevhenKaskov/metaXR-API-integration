using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System;

public class ShowKeyboard : MonoBehaviour
{
    [Header("Keyboard Placement")]
    public Transform keyboardAnchor;

    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSelect.AddListener(_ => OpenKeyboard());
    }

    private void OpenKeyboard()
    {
        var keyboard = NonNativeKeyboard.Instance;
        if (keyboard == null || keyboardAnchor == null)
            return;

        // Move keyboard
        Vector3 pos = keyboardAnchor.position;
        keyboard.transform.position = pos;

        // Rotate keyboard to match anchor Y rotation
        Vector3 euler = keyboardAnchor.eulerAngles;
        keyboard.transform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z);

        // Assign target input field
        keyboard.InputField = inputField;

        // Show keyboard
        keyboard.PresentKeyboard(inputField.text);
        SetCaretColorAlpha(1);

        keyboard.OnClosed -= OnKeyboardClosed;
        keyboard.OnClosed += OnKeyboardClosed;
    }

    private void OnKeyboardClosed(object sender, EventArgs e)
    {
        SetCaretColorAlpha(0);
        NonNativeKeyboard.Instance.OnClosed -= OnKeyboardClosed;
    }

    public void SetCaretColorAlpha(float value)
    {
        inputField.customCaretColor = true;
        Color caretColor = inputField.caretColor;
        caretColor.a = value;
        inputField.caretColor = caretColor;
    }
}
