using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerScript : MonoBehaviour
{

    public UnityEvent onEntry;
    public UnityEvent onExit;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            onEntry.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            onExit.Invoke();
        }
    }
}
