using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Potions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Player
{
    public class PlayerActions : MonoBehaviour
    {
        public CraftsList.Resources[] currentResources = new CraftsList.Resources[2];
        
        [SerializeField] private CraftsList craftsList;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private float launchForce;
        [SerializeField] private GameObject pointPrefab;
        [SerializeField] private int pointsCount;
        [SerializeField] private float deathTimer;

        private GameObject[] points;
        public Transform lastCheckpoint;
        private bool invincible;
        
        private bool canCollect;
        private int resourceIndex = 0;
        private ResourceScript resourceToCollect;
        private IPotion[] potions = new IPotion[3];
        private Vector2 projectileDirection;

        public Queue<Vector3> playerPositions = new Queue<Vector3>();
        public static PlayerActions Instance;
        private Camera cam;
		private Animator animator;

        int selectedPotion;

        bool canAdd;
        bool hasOnePotion;

        //Slider des potions
        [SerializeField] Slider jumpSlider;
        bool doubleJump;
        float jumpSliderValue;
        
        [SerializeField] Slider speedSlider;
        bool speedBoost;
        float speedSliderValue;

        private void OnEnable()
        {
            PauseMenu.OnPause.AddListener(() => enabled = false);
            PauseMenu.OnResume.AddListener(() => enabled = true);
        }

        private void OnDisable()
        {
            PauseMenu.OnPause.RemoveListener(() => enabled = false);
            PauseMenu.OnResume.RemoveListener(() => enabled = true);
        }
        
        private void Start()
        {
            jumpSlider.gameObject.SetActive(false);
            speedSlider.gameObject.SetActive(false);

            animator = GetComponent<Animator>();
            points = new GameObject[pointsCount];
            Transform previewParent = new GameObject("Shot trajectory preview").transform;

            for (int i = 0; i < pointsCount; i++)
            {
                points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity, previewParent);
            }
            
            Instance = this;
            cam = Camera.main;
        }

        private void Update()
        {
            HandleSliders();

            //detect if the player has at least one potion
            if (potions[0] == null && potions[1] == null && potions[2] == null)
                hasOnePotion = false;
            else
                hasOnePotion = true;

            //detect if we can add a potion
            if (potions[0] != null && potions[1] != null && potions[2] != null)
                canAdd = false;
            else
                canAdd = true;

            HandleInputs();

            SavePlayerPosition();

            Scroll();
        }

        private void HandleSliders()
        {
            jumpSliderValue -= 1 * Time.deltaTime;
            jumpSlider.value = jumpSliderValue;

            speedSliderValue -= 1 * Time.deltaTime;
            speedSlider.value = speedSliderValue;

            if (GetComponent<PlayerController>().CanDoubleJump && !doubleJump)
                StartCoroutine(JumpSlider());
            if (GetComponent<PlayerController>().moveClamp > 13f && !speedBoost)
                StartCoroutine(SpeedSlider());
        }
        
        private void HandleInputs()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Collect();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && hasOnePotion && potions[selectedPotion] != null &&
                !potions[selectedPotion].IsActive)
            {
                potions[selectedPotion].Drink(playerController, this);
                potions[selectedPotion] = null;
                CheckRecipe();
                if (onThrow != null)
                    onThrow.Invoke(selectedPotion);
            }

            HandleThrow();

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Death());
            }
        }

        private void HandleThrow()
        {
            projectileDirection = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            
            if (Input.GetKey(KeyCode.Mouse0) && hasOnePotion)
            {
                for (int i = 0; i < pointsCount; i++)
                {
                    points[i].transform.position = PointPosition(i * 0.1f);
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && hasOnePotion && potions[selectedPotion] != null &&
                !potions[selectedPotion].IsActive)
            {
                GameObject projectileGO = Instantiate(potions[selectedPotion].Throw(), transform.position, Quaternion.identity);
                potions[selectedPotion] = null;
                projectileGO.GetComponent<Rigidbody2D>().velocity = projectileDirection * (1.5f * launchForce);
                CheckRecipe();
                if (onThrow != null)
                    onThrow.Invoke(selectedPotion);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<ResourceScript>())
            {
                canCollect = true;
                resourceToCollect = other.GetComponent<ResourceScript>();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (canCollect && other.gameObject == resourceToCollect.gameObject)
            {
                canCollect = false;
            }
        }
        
        IEnumerator JumpSlider()
        {
            jumpSlider.gameObject.SetActive(true);
            jumpSlider.maxValue = 4;
            jumpSlider.value = jumpSlider.maxValue;
            jumpSliderValue = 4;
            doubleJump = true;
            yield return new WaitForSeconds(4);
            doubleJump = false;
            jumpSlider.gameObject.SetActive(false);
        }

        IEnumerator SpeedSlider()
        {
            speedSlider.gameObject.SetActive(true);
            speedSlider.maxValue = 4f;
            speedSlider.value = jumpSlider.maxValue;
            speedSliderValue = 4f;
            speedBoost = true;
            yield return new WaitForSeconds(4);
            speedBoost = false;
            speedSlider.gameObject.SetActive(false);
        }
        
        void Scroll()
        {
            if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                selectedPotion--;
                if (selectedPotion == -1)
                    selectedPotion = 2;
            }
            if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                selectedPotion++;
                if (selectedPotion == 3)
                    selectedPotion = 0;
            }

            if (Input.GetKeyDown(KeyCode.Q)) selectedPotion = 2;
            if (Input.GetKeyDown(KeyCode.W)) selectedPotion = 1;
            if (Input.GetKeyDown(KeyCode.E)) selectedPotion = 0;

            onSelect.Invoke(selectedPotion);

        }

        private void Collect()
        {
            if (!canAdd)
                return;

            if (canCollect)
            {
                if (currentResources.Contains(resourceToCollect.resourceType)) return;
                
                currentResources[resourceIndex] = resourceToCollect.resourceType;

                if (onCollect != null)
                {
                    onCollect.Invoke(resourceIndex, resourceToCollect.resourceType);
                }
                
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
            if (!canAdd)
                return;

            foreach (var recipe in craftsList.recipes)
            {
                if (!recipe.ingredients.Contains(currentResources[0])) continue;
                
                if (!recipe.ingredients.Contains(currentResources[1])) continue;

                for (int i = 0; i < potions.Length; i++)
                {
                    if (potions[i] == null)
                    {
                        potions[i] = ((IPotion)recipe.output);

                        if (onRecipeComplete != null)
                        {
                            onRecipeComplete.Invoke((IPotion)recipe.output, i);
                        }
                        break;
                    }
                }
                //potions.Add((IPotion) recipe.output);
                
                for (int i = 0; i < 2; i++)
                {
                    currentResources[i] = CraftsList.Resources.None;
                }
            }
        }

        public IEnumerator Death()
        {
            animator.Play("PlayerDeath");
            playerController.enabled = false;
            yield return new WaitForSeconds(deathTimer);
            /*playerController.enabled = true;
            animator.Play("PlayerIdle");
            transform.position = lastCheckpoint.position;
            for(int i = 0; i < potions.Length; i++)
            {
                potions[i] = null;
                if (onThrow != null)
                    onThrow.Invoke(i);
            }
            for (int i = 0; i < 2; i++)
            {
                currentResources[i] = CraftsList.Resources.None;
            }*/

            onDeath();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        private void SavePlayerPosition()
        {
            playerPositions.Enqueue(transform.position);
        }

        private Vector2 PointPosition(float t)
        {
            Vector2 currentPosition = (Vector2) transform.position + projectileDirection * (launchForce * t) + Physics2D.gravity *
                (.5f * t * t);
            return currentPosition;
        }

        public delegate void PlayerCallback(int resourceIndex, CraftsList.Resources resourceType);
        public delegate void PlayerCallback2(IPotion potion, int slot);
        public delegate void PlayerCallback3(int slot);
        public delegate void PlayerCallback4(int slot);
        public delegate void PlayerCallback5();

        public static PlayerCallback onCollect;
        public static PlayerCallback2 onRecipeComplete;
        public static PlayerCallback3 onThrow;
        public static PlayerCallback4 onSelect;
        public static PlayerCallback5 onDeath;
    }
    
}
