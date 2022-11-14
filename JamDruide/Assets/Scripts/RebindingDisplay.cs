using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class RebindingDisplay : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpAction = null;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private TextMeshProUGUI bindingDisplay;
    [SerializeField] private GameObject startRebind;
    [SerializeField] private Sprite space;
    [SerializeField] private Sprite other;
    [SerializeField] private GameObject waiting;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void StartRebinding()
    {
        startRebind.SetActive(false);
        waiting.SetActive(true);

        foreach (KeyControl key in Keyboard.current.allKeys)
        {
            Debug.Log(key.displayName);
        }

        PlayerController.Controls.Movement.Jump.GetBindingDisplayString(
            PlayerController.Controls.Movement.Jump.GetBindingIndexForControl(PlayerController.Controls.Movement
                .Jump.controls[2]), out string deviceLayout, out string controlPath);
        
        Debug.Log(deviceLayout);
        Debug.Log(controlPath);
        Debug.Log(PlayerController.Controls.Movement.Jump.controls[2].displayName);
        
        PlayerController.Controls.Disable();
        playerInput.enabled = false;
        rebindingOperation = PlayerController.Controls.Movement.Jump
            .PerformInteractiveRebinding(
                PlayerController.Controls.Movement.Jump.GetBindingIndexForControl(PlayerController.Controls.Movement
                    .Jump.controls[2])).OnMatchWaitForAnother(0.1f).OnComplete(_ =>
            {
                startRebind.SetActive(true);
                waiting.SetActive(false);
                rebindingOperation.Dispose();
                PlayerController.Controls.Enable();
                playerInput.enabled = true;
                if (PlayerController.Controls.Movement.Jump.controls[2].displayName == Keyboard.current.spaceKey.displayName)
                {
                    startRebind.GetComponent<Image>().sprite = space;
                }
                else
                {
                    startRebind.GetComponent<Image>().sprite = other;
                }
            }).Start();
    }
}