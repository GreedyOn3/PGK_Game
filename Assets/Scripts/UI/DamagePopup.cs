using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DamagePopup : MonoBehaviour
    {
        public float floatSpeed = 2.0f;
        public float lifetime = 1.0f;
        public float fadeSpeed = 3.0f;

        private TextMeshProUGUI _text;
        private float _lifetimeTimer;
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
            if (_lifetimeTimer > lifetime)
            {
                Destroy(gameObject);
            }

            _lifetimeTimer += Time.deltaTime;

            transform.position += Vector3.up * (floatSpeed * Time.deltaTime);
            var textColor = _text.color;
            textColor.a -= fadeSpeed * Time.deltaTime;
            _text.color = textColor;

            if (_camera == null)
            {
                return;
            }

            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, Vector3.up);
        }

        public void SetDamage(int damage)
        {
            _text.text = $"{damage}";
        }
    }
}
