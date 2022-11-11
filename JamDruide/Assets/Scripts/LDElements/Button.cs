using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{

    public GameObject door;
    bool isOpen;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
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
