using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "KeyCodeImage", menuName = "Scriptable Objects/KeyCodeImage", order = 0)]
    public class KeyCodeImage : ScriptableObject
    {
        public Sprite[] sprites = new Sprite[111];
    }
}