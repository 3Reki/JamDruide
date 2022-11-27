using UnityEngine;

namespace LDElements
{
    public class Spike : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                StartCoroutine(Player.PlayerActions.Instance.Death());
            }
        }
    }
}
