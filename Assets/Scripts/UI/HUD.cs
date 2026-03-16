using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public Slider healthSlider;
    public Slider xpSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI statsText;

    public PlayerReferences player;

    private void Update()
    {
        healthSlider.value = player.health.value;
        healthSlider.maxValue = player.health.maxValue;
        xpSlider.value = player.xp.value;
        xpSlider.maxValue = player.xp.maxValue;
        levelText.text = $"Level {player.xp.Level}";
        statsText.text = $"Player stats:\nMovement speed: {player.stats.movementSpeed}\nAttack: {player.stats.attack}\nDefense: {player.stats.defense}";
    }
}
