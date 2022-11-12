using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{
    private CircleCollider2D collider2D;
    public CraftsList.Resources resourceType;
    [SerializeField] private float resourceSpawnTime;
    Animator animator;

    private void Start()
    {
        collider2D = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void ResourceCollected()
    {
        collider2D.enabled = false;
        StartCoroutine(NewResource());
    }

    private IEnumerator NewResource()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if(CraftsList.Resources.Mushroom == resourceType)
            animator.Play("PickUpMush");
        if (CraftsList.Resources.Salt == resourceType)
            animator.Play("PickUp");
        if (CraftsList.Resources.Hydromel == resourceType)
            animator.Play("PickUpHydro");

        yield return new WaitForSeconds(resourceSpawnTime);

        if (CraftsList.Resources.Mushroom == resourceType)
            animator.Play("RespawnMush");
        if (CraftsList.Resources.Salt == resourceType)
            animator.Play("Respawn");
        if (CraftsList.Resources.Hydromel == resourceType)
            animator.Play("RespawnHydro");

        
        sprite.enabled = true;
        collider2D.enabled = true;
    }
}
