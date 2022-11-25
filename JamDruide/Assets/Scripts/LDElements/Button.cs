using UnityEngine;

namespace LDElements
{
    public class Button : MonoBehaviour
    {

        public GameObject door;
        public Sprite brokenSprite;
	    [SerializeField] private GameObject explosion;
        private SpriteRenderer _spriteRenderer;
        private bool isOpen;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Damage"))
            {
                if(!isOpen)
                    OpenDoor();
            }
        }

        void OpenDoor()
        {
            _spriteRenderer.sprite = brokenSprite;
            isOpen = true;
            door.GetComponent<Animator>().Play("DoorOpen");
	        Instantiate(explosion, transform.position, Quaternion.identity);
        }

    }
}
