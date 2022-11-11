using Player;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "Levitation Potion", menuName = "Scriptable Objects/Levitation Potion", order = 0)]
    public class LevitationPotion : AbstractPotion
    {
        public override void Drink(PlayerController player)
        {
            throw new System.NotImplementedException();
        }
    }
}