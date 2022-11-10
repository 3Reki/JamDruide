using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Potions
{
    public class DashPotion : ScriptableObject, IPotion
    {
        public Image image;
        
        public void Drink(PlayerController player)
        {
            player.CanDash = false;
        }

        public void Throw()
        {
            throw new System.NotImplementedException();
        }
    }
}
