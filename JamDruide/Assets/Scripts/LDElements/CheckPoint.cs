using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Check(collision);
    }

    void Check(Collider2D col)
    {
        col.GetComponent<Player.PlayerActions>().lastCheckpoint = gameObject.transform;
        Debug.Log("oui");
    }
}
