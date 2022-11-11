using System.Collections;
using Player;
using UnityEditor;
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
            var newParticleSystem = Instantiate(particleSystem, PlayerActions.Instance.transform);
            yield return new WaitForSeconds(effectDuration);
            Destroy(newParticleSystem);
            player.CanDoubleJump = false;
            IsActive = false;
        }
    }
}