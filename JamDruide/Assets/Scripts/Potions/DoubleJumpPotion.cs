using System.Collections;
using Player;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "Double Jump Potion", menuName = "Scriptable Objects/Double Jump Potion", order = 0)]
    public class DoubleJumpPotion : AbstractPotion
    {
        public new bool IsActive { get; private set; }

        [SerializeField] private float effectDuration;
        
        public override void Drink(PlayerController player, PlayerActions playerActions)
        {
            player.StartCoroutine(EffectCoroutine(player));
        }

        private IEnumerator EffectCoroutine(PlayerController player)
        {
            player.CanDoubleJump = true;
            IsActive = true;
            
            yield return new WaitForSeconds(effectDuration);
            
            player.CanDoubleJump = false;
            IsActive = false;
        }
    }
}