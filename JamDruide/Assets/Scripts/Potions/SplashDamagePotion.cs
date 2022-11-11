using UnityEngine;

namespace Potions
{
    public class SplashDamagePotion : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            Destroy(gameObject);
        }
    }
}