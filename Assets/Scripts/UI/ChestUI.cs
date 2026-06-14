using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    public static ChestUI Instance { get; private set; }

    [SerializeField] private GameObject windowParent;
    [Header("UI Elements")]
    [SerializeField] private List<RectTransform> uiElements = new List<RectTransform>();
    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescText;
    [SerializeField] private Image itemIcon;

    [SerializeField] private CanvasGroup flashCanvasGroup;
    [SerializeField] private Animator chestAnimator;
    [Header("Animation Settings")]
    [SerializeField] private float startOpenTime = 1f;
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine _activeAnimation;
    private Coroutine _openDelayRoutine;
    private LevelManager _levelManager;
    private InputManager _inputManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        _levelManager = LevelManager.Instance;
        _inputManager = InputManager.Instance;
        windowParent.SetActive(false);
    }

    public void OpenChest(ChoiceRarity rarity, SpecialItemInfo itemInfo)
    {
        windowParent.SetActive(true);
        _levelManager.PauseLevel();
        _inputManager.SwitchInputMode(InputMode.Ui);

        SetState(Vector3.zero, 0f);
        rarityText.SetText(rarity.Name);
        rarityText.color = rarity.Color;

        itemNameText.SetText(itemInfo.Name);
        itemDescText.SetText(itemInfo.Description);
        itemIcon.sprite = itemInfo.Image;

        chestAnimator.SetTrigger("Fall");

        if (_openDelayRoutine != null) 
            StopCoroutine(_openDelayRoutine);
        _openDelayRoutine = StartCoroutine(WaitAndStartOpen());
    }

    private IEnumerator WaitAndStartOpen()
    {
        yield return new WaitForSecondsRealtime(startOpenTime);
        StartOpen();
    }

    public void CloseWindow()
    {
        windowParent.SetActive(false);
        _levelManager.UnpauseLevel();
        _inputManager.SwitchInputMode(InputMode.Gameplay);
    }

    private void StartOpen()
    {
        if(flashCanvasGroup) flashCanvasGroup.alpha = 1f;
        chestAnimator.SetTrigger("Open");

        if (_activeAnimation != null)
            StopCoroutine(_activeAnimation);
        _activeAnimation = StartCoroutine(RevealAnimRoutine(Vector3.one, 0f));
    }

    private IEnumerator RevealAnimRoutine(Vector3 targetScale, float targetAlpha)
    {
        float initialAlpha = (flashCanvasGroup) ? flashCanvasGroup.alpha : 0f;

        float elapsedTime = 0f;
        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float scaleEval = scaleCurve.Evaluate(elapsedTime / scaleDuration);
            float fadeEval = fadeCurve.Evaluate(elapsedTime / fadeDuration);

            for (int i = 0; i < uiElements.Count; i++)
            {
                if (uiElements[i] == null) continue;
                uiElements[i].localScale = Vector3.LerpUnclamped(Vector3.zero, targetScale, scaleEval);
            }
            if(flashCanvasGroup) flashCanvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, fadeEval);

            yield return null;
        }

        SetState(targetScale, targetAlpha);
    }

    private void SetState(Vector3 targetScale, float targetAlpha)
    {
        foreach (RectTransform element in uiElements)
            if (element != null) 
                element.localScale = targetScale;

        if (flashCanvasGroup) flashCanvasGroup.alpha = targetAlpha;
    }
}
