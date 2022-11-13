using UnityEngine;

namespace Potions
{
    public class SplashJumpPotion : MonoBehaviour
    {
        [SerializeField] private GameObject particles;
        [SerializeField] private new AudioSource audio;
        [SerializeField] AudioClip explose;
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
            audio.PlayOneShot(explose);
            Instantiate(particles, transform.position, Quaternion.identity);
            collider2D.enabled = false;
            Destroy(spriteRenderer);
            Destroy(rigidbody2D);
            Destroy(trailRenderer);
            Destroy(gameObject, 2);
        }
    }
}