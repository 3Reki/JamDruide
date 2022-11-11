using Player;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "Levitation Potion", menuName = "Scriptable Objects/Levitation Potion", order = 0)]
    public class LevitationPotion : ScriptableObject, IPotion
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