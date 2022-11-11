using Player;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "AoE Potion", menuName = "Scriptable Objects/AoE Potion", order = 0)]
    public class AoEPotion : ScriptableObject, IPotion
    {
        public bool IsActive => false;
        
        public Sprite sprite;

        public void Drink(PlayerController player)
        {
            throw new System.NotImplementedException();
        }

        public void Throw()
        {
            throw new System.NotImplementedException();
        }
        
    }
}