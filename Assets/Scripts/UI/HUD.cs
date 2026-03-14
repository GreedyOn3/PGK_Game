using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public Slider healthSlider;
    public Slider xpSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI statsText;

    public Health playerHealth;
    public PlayerXp playerXp;
    public PlayerStats playerStats;

    private void Update()
    {
        healthSlider.value = playerHealth.value;
        healthSlider.maxValue = playerHealth.maxValue;
        xpSlider.value = playerXp.value;
        xpSlider.maxValue = playerXp.maxValue;
        levelText.text = $"Level {playerXp.Level}";
        statsText.text = $"Player stats:\nMovement speed: {playerStats.movementSpeed}\nAttack: {playerStats.attack}\nDefense: {playerStats.defense}";
    }
}
