using UnityEngine;

namespace LDElements
{
    public class SpeedPlatform : MonoBehaviour
    {
        public bool IsActive;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float platformSpeed;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Speed"))
            {
                IsActive = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!IsActive)
            {
                return;
            }
            if (col.gameObject.CompareTag("Player"))
            {
                if (col.GetContact(0).normal == Vector2.right)
                {
                    IsActive = false;
                    rb.velocity = Vector2.right * platformSpeed;
                    return;
                }
                
                if (col.GetContact(0).normal == Vector2.left)
                {
                    IsActive = false;
                    rb.velocity = Vector2.left * platformSpeed;
                }
            }
        }
    }
}