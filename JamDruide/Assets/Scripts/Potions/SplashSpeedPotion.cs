using UnityEngine;

namespace Potions
{
    public class SplashSpeedPotion : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            Destroy(gameObject);
        }
    }
}