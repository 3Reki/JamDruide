using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Sprite List", menuName = "Scriptable Objects/Sprite List", order = 0)]
    public class SpriteArray : ScriptableObject
    {
        public Sprite[] sprites = new Sprite[111];
    }
}