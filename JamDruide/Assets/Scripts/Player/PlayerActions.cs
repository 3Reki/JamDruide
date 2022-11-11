using System.Collections.Generic;
using System.Linq;
using Potions;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerActions : MonoBehaviour
    {
        public CraftsList.Resources[] currentResources = new CraftsList.Resources[2];
        
        [SerializeField] private CraftsList craftsList;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private List<Sprite> resourceImages;
        [SerializeField] private List<Image> CraftUI;
        [SerializeField] private GameObject projectile;
        [SerializeField] private float launchForce;
        [SerializeField] private GameObject pointPrefab;
        [SerializeField] private int pointsCount;

        private GameObject[] points;
        private bool canCollect;
        private int resourceIndex = 0;
        private ResourceScript resourceToCollect;
        private List<IPotion> potions = new();
        private Dictionary<CraftsList.Resources, Sprite> resourcesImages;
        private Vector2 projectileDirection;

        private void Start()
        {
            points = new GameObject[pointsCount];

            for (int i = 0; i < pointsCount; i++)
            {
                points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity);
            }
            
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

        private void Update()
        {
            projectileDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

            for (int i = 0; i < pointsCount; i++)
            {
                points[i].transform.position = PointPosition(i * 0.1f);
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                Collect();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && potions.Count != 0 && !potions[0].IsActive)
            {
                potions[0].Drink(playerController);
                potions.RemoveAt(0);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0)) // && potions.Count != 0 && !potions[0].IsActive)
            {
                GameObject projectileGO = Instantiate(projectile, transform.position, Quaternion.identity);
                projectileGO.GetComponent<Rigidbody2D>().velocity =
                    projectileDirection * launchForce;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<ResourceScript>())
            {
                canCollect = false;
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
            foreach (var recipe in craftsList.recipes)
            {
                if (!recipe.ingredients.Contains(currentResources[0])) continue;
                
                if (!recipe.ingredients.Contains(currentResources[1])) continue;
                    
                potions.Add((IPotion) recipe.output);
                for (int i = 0; i < 2; i++)
                {
                    currentResources[i] = CraftsList.Resources.None;
                    CraftUI[i].sprite = resourcesImages[CraftsList.Resources.None];
                }
            }
        }

        private Vector2 PointPosition(float t)
        {
            Vector2 currentPosition = (Vector2) transform.position + projectileDirection * (launchForce * t) + Physics2D.gravity *
                (.5f * t * t);
            return currentPosition;
        }
    }
}
