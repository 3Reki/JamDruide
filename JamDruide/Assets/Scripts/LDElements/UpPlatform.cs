using UnityEngine;

namespace LDElements
{
    public class UpPlatform : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float platformSpeed;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("JumpPot"))
            {
                rb.velocity = Vector2.up * platformSpeed;
            }
        }
    }
}
