using Player;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "Damage Potion", menuName = "Scriptable Objects/Damage Potion", order = 0)]
    public class DamagePotion : AbstractPotion
    {

        public override void Drink(PlayerController player)
        {
            // TODO kill player
            throw new System.NotImplementedException();
        }
    }
}