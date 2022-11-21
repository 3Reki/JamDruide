using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindingDisplay : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject waiting;
    [SerializeField] private RebindGroup[] rebindGroups;
    [SerializeField] private SpriteArrayList spriteLists;

    private void Start()
    {
        playerInput = PlayerActions.Instance.GetComponent<PlayerInput>();
        RefreshKeys();
    }

    public void ResetAllBindings()
    {
        foreach (InputActionMap actionMap in PlayerController.Controls.asset.actionMaps)
        {
            actionMap.RemoveAllBindingOverrides();
        }

        RefreshKeys();
        OnRebind?.Invoke();
        
        PlayerPrefs.DeleteKey("rebinds");
    }

    private void RefreshKeys()
    {
        foreach (RebindGroup rebindGroup in rebindGroups)
        {
            InputControl control = PlayerController.Controls.FindAction(rebindGroup.inputActionName)
                .controls[rebindGroup.controlNbr];
            if (control.device.name == "Mouse")
            {
                MouseRebind(control.displayName, rebindGroup);
            }
            else
            {
                KeyboardRebind(control.displayName, rebindGroup);
            }
        }
    }

    public void StartRebinding(int rebindGroupIndex)
    {
        RebindGroup group = rebindGroups[rebindGroupIndex];
        InputAction inputAction = PlayerController.Controls.FindAction(group.inputActionName);
        group.modifiedBinding.SetActive(false);
        waiting.SetActive(true);

        PlayerController.Controls.Disable();
        playerInput.enabled = false;
        inputAction.PerformInteractiveRebinding(inputAction.GetBindingIndexForControl(inputAction.controls[group.controlNbr]))
            .OnMatchWaitForAnother(0.1f).WithCancelingThrough("<Keyboard>/escape").OnComplete(rebindingOperation =>
            {
                group.modifiedBinding.SetActive(true);
                waiting.SetActive(false);
                PlayerController.Controls.Enable();
                playerInput.enabled = true;

                if (rebindingOperation.selectedControl.device.name == "Mouse")
                {
                    MouseRebind(rebindingOperation, group);
                }
                else
                {
                    KeyboardRebind(rebindingOperation, group);
                }

                OnRebind?.Invoke();
                rebindingOperation.Dispose();
            }).OnCancel(rebindingOperation =>
            {
                group.modifiedBinding.SetActive(true);
                waiting.SetActive(false);

                PlayerController.Controls.Enable();
                playerInput.enabled = true;
                rebindingOperation.Dispose();
            }).Start();
    }

    private void KeyboardRebind(InputActionRebindingExtensions.RebindingOperation rebindingOperation, RebindGroup group) =>
        KeyboardRebind(rebindingOperation.selectedControl.displayName, group);

    private void KeyboardRebind(string displayName, RebindGroup group)
    {
        Sprite sprite = spriteLists.GetKeyboardSprite(displayName);

        if (sprite != null)
        {
            group.bindingImage.sprite = sprite;
            group.bindingImage.enabled = true;
            group.bindingText.enabled = false;
        }
        else
        {
            group.bindingImage.enabled = false;
            group.bindingText.text = displayName;
            group.bindingText.enabled = true;
        }
    }

    private void MouseRebind(InputActionRebindingExtensions.RebindingOperation rebindingOperation, RebindGroup group) =>
        MouseRebind(rebindingOperation.selectedControl.displayName, group);

    private void MouseRebind(string displayName, RebindGroup group)
    {
        Sprite sprite = spriteLists.GetMouseSprite(displayName);
        if (sprite != null)
        {
            group.bindingImage.sprite = sprite;
        }
        else
        {
            group.bindingImage.enabled = false;
            group.bindingText.text = displayName;
            group.bindingText.enabled = true;
            return;
        }
        group.bindingImage.enabled = true;
        group.bindingText.enabled = false;
    }

    [Serializable]
    private struct RebindGroup
    {
        public string inputActionName;
        public int controlNbr;
        public Image bindingImage;
        public GameObject modifiedBinding;
        public TextMeshProUGUI bindingText;
    }

    public static event Action OnRebind;
}

