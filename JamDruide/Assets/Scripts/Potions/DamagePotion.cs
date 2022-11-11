using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Potions
{
    [CreateAssetMenu(fileName = "Damage Potion", menuName = "Scriptable Objects/Damage Potion", order = 0)]
    public class DamagePotion : ScriptableObject, IPotion
    {
        public bool IsActive => false;
        
        public Sprite Sprite => _sprite;

        [SerializeField] private Sprite _sprite;

        public void Drink(PlayerController player)
        {
            // TODO kill player
            throw new System.NotImplementedException();
        }

        public void Throw()
        {
            throw new System.NotImplementedException();
        }
        
    }
}