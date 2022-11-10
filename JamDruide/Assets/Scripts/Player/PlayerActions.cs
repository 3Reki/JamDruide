using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public CraftsList.Resources[] currentResources = new CraftsList.Resources[2];
    private CraftsList.Resources resourceToCollect;
    private bool canCollect;
    private int resourceIndex = 0;
    [SerializeField] private CraftsList craftsList;

    [SerializeField] private List<GameObject> potions;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ResourceScript>())
        {
            Debug.Log("j'ai une ressource à proximité");
            canCollect = true;
            ResourceScript script = other.GetComponent<ResourceScript>();
            resourceToCollect = script.resourceType;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<ResourceScript>())
        {
            canCollect = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    private void Collect()
    {
        Debug.Log("Je récupère une ressource" + resourceIndex);
        if (canCollect)
        {
            if (currentResources.Contains(resourceToCollect)) return;
            currentResources[resourceIndex] = resourceToCollect;
            resourceIndex++;
            if (resourceIndex == 2)
            {
                resourceIndex = 0;
                CheckRecipe();
            }
        }
    }

    private void CheckRecipe()
    {
        Debug.Log("Je check la recette");
        foreach (var recipe in craftsList.recipes)
        {
            if (recipe.ingredients.Contains(currentResources[0]))
            {
                if (recipe.ingredients.Contains(currentResources[1]))
                {
                    potions.Add(recipe.output);
                    currentResources[0] = CraftsList.Resources.None;
                    currentResources[1] = CraftsList.Resources.None;
                }
            }
        }
    }
}
