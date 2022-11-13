using UnityEngine;

namespace Potions
{
    public class SplashDamagePotion : MonoBehaviour
    {
        [SerializeField] private new GameObject particleSystem;
        [SerializeField] private new AudioSource audio;
        [SerializeField] private AudioClip explose;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SerializeField] private new Collider2D collider2D;
        [SerializeField] private TrailRenderer trailRenderer;

        private bool activated;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (activated)
                return;

            activated = true;
            Instantiate(particleSystem, transform.position, Quaternion.identity);
            collider2D.enabled = false;
            audio.PlayOneShot(explose);
            Destroy(spriteRenderer);
            Destroy(rigidbody2D);
            Destroy(trailRenderer);
            Destroy(gameObject, 2);
        }
    }
}