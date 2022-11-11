using System;
using UnityEngine;

namespace LDElements
{
    public class Button : MonoBehaviour
    {

        public GameObject door;
        bool isOpen;
        public Sprite brokenSprite;
        private SpriteRenderer _spriteRenderer;

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
        }

    }
}
