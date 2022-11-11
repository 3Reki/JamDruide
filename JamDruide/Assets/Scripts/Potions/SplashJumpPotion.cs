using UnityEngine;

namespace Potions
{
    public class SplashJumpPotion : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            Destroy(gameObject);
        }
    }
}