using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindingDisplay : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private SpriteArray defaultKeySprites;
    [SerializeField] private SpriteArray azertyKeySprites;
    [SerializeField] private SpriteArray mouseSprites;
    [SerializeField] private GameObject waiting;
    [SerializeField] private RebindGroup[] rebindGroups;

    private void Start()
    {
        playerInput = PlayerActions.Instance.GetComponent<PlayerInput>();
    }

    public void ResetAllBindings()
    {
        foreach (InputActionMap actionMap in PlayerController.Controls.asset.actionMaps)
        {
            actionMap.RemoveAllBindingOverrides();
        }

        foreach (RebindGroup rebindGroup in rebindGroups)
        {
            InputControl control = PlayerController.Controls.FindAction(rebindGroup.inputActionName).controls[rebindGroup.controlNbr];
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
        Key code = Keyboard.current.FindKeyOnCurrentKeyboardLayout(displayName).keyCode;

        SpriteArray currentSpriteList =
            GetCurrentLayout() == KeyboardLayout.Azerty ? azertyKeySprites : defaultKeySprites;

        if (currentSpriteList.sprites[(int) code] != null)
        {
            group.bindingImage.sprite = currentSpriteList.sprites[(int) code];
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
        if (displayName == Mouse.current.leftButton.displayName)
        {
            group.bindingImage.sprite = mouseSprites.sprites[0];
        } 
        else if (displayName == Mouse.current.middleButton.displayName)
        {
            group.bindingImage.sprite = mouseSprites.sprites[1];
        } 
        else if (displayName == Mouse.current.rightButton.displayName)
        {
            group.bindingImage.sprite = mouseSprites.sprites[2];
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

    public static KeyboardLayout GetCurrentLayout()
    {
        return Keyboard.current.qKey.displayName == "A" && Keyboard.current.wKey.displayName == "Z"
            ? KeyboardLayout.Azerty
            : KeyboardLayout.Qwerty;
    }

    public enum KeyboardLayout
    {
        Qwerty,
        Azerty
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
}

