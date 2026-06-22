using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class ShopOptionUI : MonoBehaviour
    {
        [SerializeField] private int price = 12;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI rarityText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField] private Image resourceImage;
        [SerializeField] private TextMeshProUGUI priceText;

        private PlayerReferences _player;
        private LevelUpChoice _choice;
        private ShopUI _shopUI;

        public void Initialize(PlayerReferences player, LevelUpChoice choice, ShopUI shopUI)
        {
            _player = player;
            _choice = choice;
            _shopUI = shopUI;

            if (choice.Rarity != null)
            {
                rarityText.text = choice.Rarity.Name;
                rarityText.color = choice.Rarity.Color;
                price = (int)(price * choice.Rarity.Multiplier);
            }
            else
            {
                rarityText.text = "";
            }

            BaseItemInfo item = choice.Item;
            image.sprite = item.Image;
            nameText.text = item.Name;

            if (choice.Type == ChoiceType.UpgradePassive || choice.Type == ChoiceType.UpgradeWeapon)
            {
                foreach (StatInfo stat in choice.Stats)
                    descriptionText.text = $"{Stat.GetDisplayName(stat.Type)} +{stat.Value}{((stat.IsPercentage) ? "%" : "")}\n";
                descriptionText.text = descriptionText.text.TrimEnd('\n');
            }
            else
                descriptionText.text = item.Description;

            resourceImage.sprite = _player.ResourceSprite;
            priceText.text = price.ToString();
        }

        public void TryBuy()
        {
            _shopUI.BuyOption(this);
        }

        public int GetPrice() => price;
        public LevelUpChoice GetChoiceInfo() => _choice;
    }
}
