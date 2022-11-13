using UnityEngine;

namespace Potions
{
    public class SplashDamagePotion : MonoBehaviour
    {
        [SerializeField] private new GameObject particleSystem;
        [SerializeField] private new AudioSource audio;
        [SerializeField] private AudioClip explose;
        private void OnTriggerEnter2D(Collider2D col)
        {
            Instantiate(particleSystem, transform.position, Quaternion.identity);
            audio.PlayOneShot(explose);
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<TrailRenderer>());
            Destroy(gameObject, 2);
        }
    }
}