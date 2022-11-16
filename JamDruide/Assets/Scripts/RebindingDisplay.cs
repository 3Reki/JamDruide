using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindingDisplay : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private KeyCodeImage defaultKeySprites;
    [SerializeField] private KeyCodeImage azertyKeySprites;
    [SerializeField] private GameObject waiting;
    [SerializeField] private RebindGroup[] rebindGroups;

    private void Start()
    {
        playerInput = PlayerActions.Instance.GetComponent<PlayerInput>();
    }

    public void StartRebinding(int rebindGroupIndex)
    {
        RebindGroup group = rebindGroups[rebindGroupIndex];
        InputAction inputAction = PlayerController.Controls.FindAction(group.inputActionName);
        group.modifiedBinding.SetActive(false);
        waiting.SetActive(true);

        PlayerController.Controls.Disable();
        playerInput.enabled = false;
        inputAction.PerformInteractiveRebinding(inputAction.GetBindingIndexForControl(inputAction.controls[0]))
            .OnMatchWaitForAnother(0.1f).OnComplete(rebindingOperation =>
            {
                group.modifiedBinding.SetActive(true);
                waiting.SetActive(false);
                PlayerController.Controls.Enable();
                playerInput.enabled = true;

                Key code = Keyboard.current.FindKeyOnCurrentKeyboardLayout(rebindingOperation.selectedControl.displayName).keyCode;

                KeyCodeImage currentSpriteList =
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
                    group.bindingText.text = rebindingOperation.selectedControl.displayName;
                    group.bindingText.enabled = true;
                }
                
                rebindingOperation.Dispose();
            }).Start();
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
        public Image bindingImage;
        public GameObject modifiedBinding;
        public TextMeshProUGUI bindingText;
    }
}

