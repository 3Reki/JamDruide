using UnityEngine;

namespace Potions
{
    public class SplashJumpPotion : MonoBehaviour
    {
        [SerializeField] private GameObject particles;
        private void OnTriggerEnter2D(Collider2D col)
        {
            Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}