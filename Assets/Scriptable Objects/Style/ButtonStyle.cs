using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "ButtonStyle", menuName = "Scriptable Objects/ButtonStyle")]
public class ButtonStyle : ScriptableObject
{
    [field: SerializeField] public Color NormalColor { get; private set; }
    [field: SerializeField] public Color HighlightedColor { get; private set; }
    [field: SerializeField] public Color PressedColor { get; private set; }
    [field: SerializeField] public Color SelectedColor { get; private set; }
    [field: SerializeField] public Color DisabledColor { get; private set; }

    [field: SerializeField] public TMP_FontAsset TextFont { get; private set; }
    [field: SerializeField] public Color TextColor { get; private set; }
}
