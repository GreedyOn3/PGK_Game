using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CardElements : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI subText;

        public Image Image => image;
        public TextMeshProUGUI MainText => mainText;
        public TextMeshProUGUI SubText => subText;
    }
}
