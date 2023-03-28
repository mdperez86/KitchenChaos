using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameInput;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI moveUpButtonText;
    [SerializeField] private TextMeshProUGUI moveDownButtonText;
    [SerializeField] private TextMeshProUGUI moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI moveRightButtonText;
    [SerializeField] private TextMeshProUGUI interactButtonText;
    [SerializeField] private TextMeshProUGUI interactAlternateButtonText;
    [SerializeField] private TextMeshProUGUI pauseButtonText;

    [SerializeField] private Transform pressToRebindKeyTransform;

    private Action onCloseButtonAction;

    private void Awake()
    {
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => { 
            Hide();
            onCloseButtonAction();
        });

        moveUpButton.onClick.AddListener(() => { RebindKey(GameInput.KeyBinding.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebindKey(GameInput.KeyBinding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebindKey(GameInput.KeyBinding.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebindKey(GameInput.KeyBinding.Move_Right); });
        interactButton.onClick.AddListener(() => { RebindKey(GameInput.KeyBinding.Interact); });
        interactAlternateButton.onClick.AddListener(() => { RebindKey(GameInput.KeyBinding.InteractAlternate); });
        pauseButton.onClick.AddListener(() => { RebindKey(GameInput.KeyBinding.Pause); });

        Instance = this;
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        UpdateVisual();
        HidePressToRebindKey();
        Hide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = SoundManager.Instance.GetVolume().ToString("SOUND EFFECTS: 0%");
        musicText.text = MusicManager.Instance.GetVolume().ToString("MUSIC: 0%");

        moveUpButtonText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Move_Up);
        moveDownButtonText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Move_Down);
        moveLeftButtonText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Move_Left);
        moveRightButtonText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Move_Right);
        interactButtonText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Interact);
        interactAlternateButtonText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.InteractAlternate);
        pauseButtonText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Pause);
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);

        closeButton.Select();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    public void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindKey(KeyBinding keyBinding)
    {
        ShowPressToRebindKey();

        GameInput.Instance.RebindKey(keyBinding, () =>
        {
            UpdateVisual();
            HidePressToRebindKey();
        });
    }
}
