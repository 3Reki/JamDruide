using System.Collections;
using Player;
using UnityEngine;

namespace Potions
{
    [CreateAssetMenu(fileName = "Speed Potion", menuName = "Scriptable Objects/Speed Potion", order = 0)]
    public class SpeedPotion : ScriptableObject, IPotion
    {
        public bool IsActive { get; private set; }
        
        public Sprite sprite;

        [Range(0, 20)] [Tooltip("Effect duration in seconds")]
        [SerializeField] private float effectDuration;
        [Range(1, 4)] [Tooltip("The multiplier applied to the player speed")]
        [SerializeField] private float multiplier;

        

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
            IsActive = true;
            float defaultMaxSpeed = player.moveClamp;
            player.moveClamp *= multiplier;
            
            yield return new WaitForSeconds(effectDuration);

            IsActive = false;
            player.moveClamp = defaultMaxSpeed;
        }
    }
}
