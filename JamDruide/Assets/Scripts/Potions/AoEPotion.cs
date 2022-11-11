using Player;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "AoE Potion", menuName = "Scriptable Objects/AoE Potion", order = 0)]
    public class AoEPotion : AbstractPotion
    {
        public override void Drink(PlayerController player)
        {
            throw new System.NotImplementedException();
        }

    }
}