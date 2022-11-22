using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LDElements
{
    public class NextLevel : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                LoadLevel();
        }
        private static void LoadLevel()
        {
			//TimerManager.Instance.SaveTime();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.1f, 0.1f, 0.3f, 0.7f);
            var transform1 = transform;
            Gizmos.DrawCube(transform1.position, transform1.lossyScale);
        }
    }
}
