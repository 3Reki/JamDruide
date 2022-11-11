using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE : MonoBehaviour
{

    bool objectDeactivate;
    [SerializeField] int timer;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("AoE"))
        {
            if (!objectDeactivate)
                StartCoroutine(DeactivateObject());
        }
    }


    IEnumerator DeactivateObject()
    {
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        objectDeactivate = true;
        sprite.enabled = false;
        yield return new WaitForSeconds(timer);
        objectDeactivate = false;
        sprite.enabled = true;
    }
}
