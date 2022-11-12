using System.Collections;
using Player;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "Speed Potion", menuName = "Scriptable Objects/Speed Potion", order = 0)]
    public class SpeedPotion : AbstractPotion
    {
        public new bool IsActive { get; private set; }

        [Range(0, 20)] [Tooltip("Effect duration in seconds")]
        [SerializeField] private float effectDuration;
        [Range(1, 4)] [Tooltip("The multiplier applied to the player speed")]
        [SerializeField] private float multiplier;

        public override void Drink(PlayerController player, PlayerActions playerActions)
        {
            player.StartCoroutine(EffectCoroutine(player));
        }

        private IEnumerator EffectCoroutine(PlayerController player)
        {
            IsActive = true;
            float defaultMaxSpeed = player.moveClamp;
            player.moveClamp *= multiplier;
            var newParticleSystem = Instantiate(particleSystem, PlayerActions.Instance.transform);
            yield return new WaitForSeconds(effectDuration);
            Destroy(newParticleSystem);
            IsActive = false;
            player.moveClamp = defaultMaxSpeed;
        }
    }
}
