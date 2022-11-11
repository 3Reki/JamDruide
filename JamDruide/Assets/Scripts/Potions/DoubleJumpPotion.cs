using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Potions
{
    [CreateAssetMenu(fileName = "Double Jump Potion", menuName = "Scriptable Objects/Double Jump Potion", order = 0)]
    public class DoubleJumpPotion : ScriptableObject, IPotion
    {
        public bool IsActive { get; private set; }
        
        public Sprite Sprite => _sprite;

        [SerializeField] private Sprite _sprite;
        
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
            IsActive = true;
            
            yield return new WaitForSeconds(effectDuration);
            
            player.CanDoubleJump = false;
            IsActive = false;
        }
    }
}