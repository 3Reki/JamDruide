using UnityEngine;

namespace Potions
{
    public class SplashJumpPotion : MonoBehaviour
    {
        [SerializeField] private GameObject particles;
        [SerializeField] AudioSource audio;
        [SerializeField] AudioClip explose;

        private void OnTriggerEnter2D(Collider2D col)
        {
            audio.PlayOneShot(explose);
            Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<TrailRenderer>());
            Destroy(gameObject,2);
        }
    }
}