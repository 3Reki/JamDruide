using DG.Tweening;
using UnityEngine;

namespace LDElements
{
    public class Ingredient : MonoBehaviour
    {
        public CraftsList.Resources resourceType;
        
        [SerializeField] private GameObject input;
        [SerializeField] private new CircleCollider2D collider2D;
        
        public void ResourceCollected()
        {
            NewResource();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                input.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                input.SetActive(false);
            }
        }

        private void NewResource()
        {
            transform.DOComplete();
            collider2D.enabled = false;
            transform.DOScale(Vector3.one * 1.5f, 0.15f).onComplete = () =>
            {
                transform.DOScale(Vector3.one, 0.2f).onComplete = () => collider2D.enabled = true;
            };
        }
    }
}
