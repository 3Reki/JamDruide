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

    }
}
