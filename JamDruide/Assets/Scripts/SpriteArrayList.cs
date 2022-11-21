using Player;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Sprite Array List", menuName = "Scriptable Objects/Sprite Array List", order = 0)]
public class SpriteArrayList : ScriptableObject
{
    [SerializeField] private SpriteArray defaultKeySprites;
    [SerializeField] private SpriteArray azertyKeySprites;
    [SerializeField] private SpriteArray mouseSprites;

    public Sprite GetSpriteForInputActionName(string inputActionName)
    {
        InputControl control = PlayerController.Controls.FindAction(inputActionName).controls[0];
        
        return control.device.name == "Mouse" ? GetMouseSprite(control.displayName) : GetKeyboardSprite(control.displayName);
    }

    public Sprite GetMouseSprite(string displayName)
    {
        if (displayName == Mouse.current.leftButton.displayName)
        {
            return mouseSprites.sprites[0];
        }

        if (displayName == Mouse.current.middleButton.displayName)
        {
            return mouseSprites.sprites[1];
        }
        
        if (displayName == Mouse.current.rightButton.displayName)
        {
            return mouseSprites.sprites[2];
        }

        return null;
    }

    public Sprite GetKeyboardSprite(string displayName)
    {
        Key code = Keyboard.current.FindKeyOnCurrentKeyboardLayout(displayName).keyCode;

        SpriteArray currentSpriteList =
            GetCurrentLayout() == KeyboardLayout.Azerty ? azertyKeySprites : defaultKeySprites;

        return currentSpriteList.sprites[(int) code];
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
}