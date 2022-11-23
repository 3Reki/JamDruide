using Player;
using UnityEngine;

public class KillEnemy : MonoBehaviour
{
    [SerializeField] private LaserEnemy laserEnemy;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage"))
        {
            laserEnemy.Death();
        }
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(collision.GetComponent<PlayerActions>().Death());
        }
        
    }
}
