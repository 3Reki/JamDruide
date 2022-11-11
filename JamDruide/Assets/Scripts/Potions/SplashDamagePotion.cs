using UnityEngine;

namespace Potions
{
    public class SplashDamagePotion : MonoBehaviour
    {
        [SerializeField] private GameObject particleSystem;
        private void OnTriggerEnter2D(Collider2D col)
        {
            Instantiate(particleSystem, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}