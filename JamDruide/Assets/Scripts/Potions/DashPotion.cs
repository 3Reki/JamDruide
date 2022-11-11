using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Potions
{
    [CreateAssetMenu(fileName = "Dash Potion", menuName = "Scriptable Objects/Dash Potion", order = 0)]
    public class DashPotion : ScriptableObject, IPotion
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
            player.CanDash = true;
            
            yield return new WaitForSeconds(effectDuration);
            
            player.CanDash = false;
        }
    }
}
