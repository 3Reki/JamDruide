using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{
    private CircleCollider2D collider2D;
    public CraftsList.Resources resourceType;
    [SerializeField] private float resourceSpawnTime;

    private void Start()
    {
        collider2D = GetComponent<CircleCollider2D>();
    }

    public void ResourceCollected()
    {
        collider2D.enabled = false;
        StartCoroutine(NewResource());
    }

    private IEnumerator NewResource()
    {
        yield return new WaitForSeconds(resourceSpawnTime);
        collider2D.enabled = true;
    }
}
