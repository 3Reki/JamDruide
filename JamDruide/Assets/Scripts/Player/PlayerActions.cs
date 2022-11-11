using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    public CraftsList.Resources[] currentResources = new CraftsList.Resources[2];
    private bool canCollect;
    private int resourceIndex = 0;
    [SerializeField] private CraftsList craftsList;
    private ResourceScript resourceToCollect;

    private Dictionary<CraftsList.Resources, Sprite> resourcesImages;
    [SerializeField] private List<Sprite> resourceImages;

    [SerializeField] private List<GameObject> potions;
    [SerializeField] private List<Image> CraftUI;

    private void Start()
    {
        resourcesImages = new Dictionary<CraftsList.Resources, Sprite>()
        {
            {
                CraftsList.Resources.Hydromel, resourceImages[0]
            },
            {
                CraftsList.Resources.Mistletoe, resourceImages[1]
            },
            {
                CraftsList.Resources.Mushroom, resourceImages[2]
            },
            {
                CraftsList.Resources.Salt, resourceImages[3]
            },
            {
                CraftsList.Resources.None, resourceImages[4]
            }
        };
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ResourceScript>())
        {
            canCollect = true;
            ResourceScript script = other.GetComponent<ResourceScript>();
            resourceToCollect = script;
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
        if (canCollect)
        {
            if (currentResources.Contains(resourceToCollect.resourceType)) return;
            currentResources[resourceIndex] = resourceToCollect.resourceType;
            CraftUI[resourceIndex].sprite = resourcesImages[resourceToCollect.resourceType];
            resourceToCollect.ResourceCollected();
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
                    for (int i = 0; i < 2; i++)
                    {
                        currentResources[i] = CraftsList.Resources.None;
                        CraftUI[i].sprite = resourcesImages[CraftsList.Resources.None];
                    }
                }
            }
        }
    }
}
