using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Potions
{
    //[CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class DoubleJumpPotion : ScriptableObject, IPotion
    {
        public Image image;
        
        [SerializeField] private float effectDuration;
        
        public void Drink(PlayerController player)
        {
            player.StartCoroutine(EffectCoroutine(player));
        }

        public void Throw()
        {
            throw new System.NotImplementedException();
        }
        
        private IEnumerator EffectCoroutine(PlayerController player)
        {
            player.CanDoubleJump = true;
            
            yield return new WaitForSeconds(effectDuration);
            
            player.CanDoubleJump = false;
        }
    }
}