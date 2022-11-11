using Player;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "Size Potion", menuName = "Scriptable Objects/Size Potion", order = 0)]
    public class SizePotion : AbstractPotion
    {
        public override void Drink(PlayerController player)
        {
            throw new System.NotImplementedException();
        }

    }
}