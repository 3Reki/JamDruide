using System;
using UnityEngine;

namespace LDElements
{
    public class Button : MonoBehaviour
    {

        public GameObject door;
        bool isOpen;

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
            isOpen = true;
            door.GetComponent<Animator>().Play("DoorOpen");
        }

    }
}
