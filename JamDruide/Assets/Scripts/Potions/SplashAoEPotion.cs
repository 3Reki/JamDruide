using UnityEngine;

namespace Potions
{
    public class SplashAoEPotion : MonoBehaviour
    {
        [SerializeField] private GameObject particleSystem;
        private void OnTriggerStay2D(Collider2D col)
        {
            Instantiate(particleSystem, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}