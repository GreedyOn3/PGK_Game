using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LevelUpUi : MonoBehaviour
    {
        [SerializeField] private GameObject optionPrefab;
        [SerializeField] private GameObject levelUpScreen;

        private PlayerReferences _player;
        private GameObject[] _options = Array.Empty<GameObject>();

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();
            gameObject.SetActive(false);
        }

        public void PickUpgrade(LevelUpChoice choice)
        {
            LevelUpSystem.Instance.ApplyChoice(choice, _player.Inventory);
            LevelManager.Instance.UnpauseLevel();
            InputManager.Instance.SwitchInputMode(InputMode.Gameplay);
            gameObject.SetActive(false);
        }

        public void Show(List<LevelUpChoice> choices)
        {
            LevelManager.Instance.PauseLevel();
            InputManager.Instance.SwitchInputMode(InputMode.Ui);
            gameObject.SetActive(true);

            foreach (var option in _options)
                Destroy(option);

            _options = new GameObject[choices.Count];
            for (var i = 0; i < choices.Count; i++)
            {
                _options[i] = Instantiate(optionPrefab, levelUpScreen.transform);
                var optionUi = _options[i].GetComponent<LevelUpOptionUi>();
                optionUi.Initialize(choices[i], this);
            }
        }
    }
}
