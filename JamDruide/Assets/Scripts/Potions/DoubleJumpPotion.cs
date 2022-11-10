using Player;
using UnityEngine;

namespace Potions
{
    //[CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class DoubleJumpPotion : ScriptableObject, IPotion
    {
        public void Drink(PlayerController player)
        {
            player.CanDoubleJump = true;
        }

        public void Throw()
        {
            throw new System.NotImplementedException();
        }
    }
}