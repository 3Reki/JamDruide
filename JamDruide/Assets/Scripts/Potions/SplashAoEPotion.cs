using UnityEngine;

namespace Potions
{
    public class SplashAoEPotion : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D col)
        {
            Destroy(gameObject);
        }
    }
}