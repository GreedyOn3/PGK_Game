using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways, RequireComponent(typeof(Button))]
public class ButtonStyleApplier : MonoBehaviour
{
    [SerializeField] private ButtonStyle style;

    private void Awake()
    {
        ApplyStyle();
    }

    private void OnValidate()
    {
        ApplyStyle();
    }

    private void ApplyStyle()
    {
        if (style == null)
            return;

        var button = GetComponent<Button>();

        if (button == null)
            return;

        var colors = button.colors;

        colors.normalColor = style.NormalColor;
        colors.highlightedColor = style.HighlightedColor;
        colors.pressedColor = style.PressedColor;
        colors.selectedColor = style.SelectedColor;
        colors.disabledColor = style.DisabledColor;

        button.colors = colors;

        var text = GetComponentInChildren<TextMeshProUGUI>();

        text.font = style.TextFont;
        text.color = style.TextColor;
    }
}
