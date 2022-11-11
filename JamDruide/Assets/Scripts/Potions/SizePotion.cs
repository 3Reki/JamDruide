using System.Collections;
using DG.Tweening;
using Player;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "Size Potion", menuName = "Scriptable Objects/Size Potion", order = 0)]
    public class SizePotion : AbstractPotion
    {
        public new bool IsActive { get; private set; }

        [Range(0, 20)] [Tooltip("Effect duration in seconds")]
        [SerializeField] private float effectDuration;
        [Range(1, 4)] [Tooltip("The multiplier applied to the player size")]
        [SerializeField] private float multiplier;
        
        public override void Drink(PlayerController player, PlayerActions playerActions)
        {
            player.StartCoroutine(EffectCoroutine(player));
        }

        private IEnumerator EffectCoroutine(PlayerController player)
        {
            IsActive = true;
            var transform = player.transform;
            Vector3 defaultSize = transform.localScale;
            transform.DOScale(transform.localScale * multiplier, 0.3f);
            
            yield return new WaitForSeconds(effectDuration);

            IsActive = false;
            transform.localScale = defaultSize;
        }
    }
}