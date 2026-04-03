using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI), typeof(Lifetime))]
    public class DamagePopup : MonoBehaviour
    {
        public float floatSpeed = 2.0f;
        public float fadeSpeed = 3.0f;

        private TextMeshProUGUI _text;
        private Camera _camera;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            transform.position += Vector3.up * (floatSpeed * Time.deltaTime);
            var textColor = _text.color;
            textColor.a -= fadeSpeed * Time.deltaTime;
            _text.color = textColor;

            if (_camera == null)
            {
                return;
            }

            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward);
        }

        public void SetDamage(int damage)
        {
            _text.text = $"{damage}";
        }

        public void SetColor(Color color)
        {
            _text.color = color;
        }
    }
}
