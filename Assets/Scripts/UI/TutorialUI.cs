using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAlternateText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI keyGamepadPauseText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChange += KitchenGameManager_OnStateChange;

        UpdateVisual();

        Show();
    }

    private void KitchenGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Interact);
        keyInteractAlternateText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.InteractAlternate);
        keyPauseText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Pause);
        keyGamepadInteractText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Gamepad_Interact);
        keyGamepadInteractAlternateText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Gamepad_InteractAlternate);
        keyGamepadPauseText.text = GameInput.Instance.GetKeyBindingText(GameInput.KeyBinding.Gamepad_Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
