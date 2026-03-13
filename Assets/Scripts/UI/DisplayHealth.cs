using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class DisplayHealth : MonoBehaviour
    {
        public Health source;

        private Slider _slider;

        private void Start()
        {
            _slider = GetComponent<Slider>();
        }

        private void Update()
        {
            _slider.value = source.value;
            _slider.maxValue = source.maxValue;
        }
    }
}
