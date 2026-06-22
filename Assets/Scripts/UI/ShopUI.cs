using System.Collections.Generic;
using UI;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private GameObject shopOptionsParent;

    private PlayerReferences _player;
    private List<GameObject> _options = new();
    private ShopInteractable _currentShop;

    public static ShopUI Instance;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();
        gameObject.SetActive(false);
    }

    public void BuyOption(ShopOptionUI shopOption)
    {
        if (_player.Stats.resourceGathered < shopOption.GetPrice()) return;

        LevelUpSystem.Instance.ApplyChoice(shopOption.GetChoiceInfo(), _player.Inventory);

        _player.Stats.resourceGathered -= shopOption.GetPrice();
        _currentShop.IncreasePurchaseCounter();
        _options.Remove(shopOption.gameObject);
        Destroy(shopOption.gameObject);
    }

    public void Show(ShopInteractable shop, List<LevelUpChoice> choices)
    {
        _currentShop = shop;

        LevelManager.Instance.PauseLevel();
        InputManager.Instance.SwitchInputMode(InputMode.Ui);
        gameObject.SetActive(true);

        foreach (GameObject option in _options)
            Destroy(option);

        _options.Clear();
        for (int i = 0; i < choices.Count; i++)
        {
            GameObject option = Instantiate(optionPrefab, shopOptionsParent.transform);

            _options.Add(option);
            ShopOptionUI optionUi = option.GetComponent<ShopOptionUI>();

            optionUi.Initialize(_player, choices[i], this);
        }
    }

    public void Hide()
    {
        LevelManager.Instance.UnpauseLevel();
        InputManager.Instance.SwitchInputMode(InputMode.Gameplay);
        gameObject.SetActive(false);
        if (_currentShop)
        {
            _currentShop.Finish();
            _currentShop = null;
        }
    }
}
