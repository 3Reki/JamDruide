using System;
using System.Collections;
using System.Linq;
using CameraScripts;
using LDElements;
using Potions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerActions : MonoBehaviour
    {
        public static PlayerActions Instance;
        public CraftsList.Resources[] currentResources = new CraftsList.Resources[2];
        public Transform lastCheckpoint;

        [SerializeField] private CraftsList craftsList;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerDeviceHandler playerDeviceHandler;
        [SerializeField] private float launchForce;
        [SerializeField] private AnimationCurve launchDistTransformation;
        [SerializeField] private GameObject pointPrefab;
        [SerializeField] private int pointsCount;
        [SerializeField] private float deathTimer;

        [Header("Sounds")] [SerializeField] AudioClip die;
        [SerializeField] AudioClip craft;
        [SerializeField] AudioClip collect;
        [SerializeField] AudioClip drink;
        [SerializeField] AudioClip throwBottle;
        private new AudioSource audio;

        private GameObject[] points;
        private bool invincible;

        private bool canCollect;
        private int resourceIndex;
        private Ingredient resourceToCollect;
        private Telescope telescope;
        private readonly IPotion[] potions = new IPotion[3];
        private Vector2 projectileDirection;
        private float projectileStrength;

        private Camera cam;
        private Animator animator;

        private int selectedPotion;
        private bool canUse;
        private bool isUsing;
        private bool canAdd;
        private bool hasOnePotion;

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


        private void Awake()
        {
            Instance = this;

#if !UNITY_EDITOR
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
#endif
        }


        private void Start()
        {
            audio = GetComponent<AudioSource>();

            animator = GetComponent<Animator>();
            points = new GameObject[pointsCount];
            Transform previewParent = new GameObject("Shot trajectory preview").transform;

            for (int i = 0; i < pointsCount; i++)
            {
                points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity, previewParent);
                points[i].SetActive(false);
            }

            cam = Camera.main;
        }

        private void Update()
        {
            CheckOnePotion();

            CheckFullPotions();

            HandleInputs();

            Scroll();
        }

        private void CheckFullPotions()
        {
            //detect if we can add a potion
            if (potions[0] != null && potions[1] != null && potions[2] != null)
                canAdd = false;
            else
                canAdd = true;
        }

        private void CheckOnePotion()
        {
            //detect if the player has at least one potion
            if (potions[0] == null && potions[1] == null && potions[2] == null)
                hasOnePotion = false;
            else
                hasOnePotion = true;
        }

        private void HandleInputs()
        {
            if (PlayerController.Controls.Action.Interact.WasPerformedThisFrame())
            {
                if (canCollect)
                {
                    Collect();
                }
                else if (canUse)
                {
                    isUsing = true;
                    telescope.StartUsing();
                }
            }

            if (PlayerController.Controls.Action.Interact.WasReleasedThisFrame())
            {
                if (isUsing)
                {
                    isUsing = false;
                    telescope.StopUsing();
                }
            }

            if (isUsing && PlayerController.Controls.Movement.Move.IsPressed())
            {
                isUsing = false;
                telescope.StopUsing();
            }

            if (PlayerController.Controls.Action.Drink.WasPressedThisFrame() && hasOnePotion &&
                potions[selectedPotion] != null &&
                !potions[selectedPotion].IsActive)
            {
                potions[selectedPotion].Drink(playerController, this);
                potions[selectedPotion] = null;
                CheckRecipe();
                audio.PlayOneShot(drink);
                if (onThrow != null)
                    onThrow.Invoke(selectedPotion);
            }

            HandleThrow();

            if (PlayerController.Controls.Action.Restart.WasPerformedThisFrame())
            {
                StartCoroutine(Death());
            }
        }

        private void HandleThrow()
        {
            if (playerDeviceHandler.currentAimMethod == PlayerDeviceHandler.AimMethod.Mouse)
            {
                MouseAim();
            }
            else
            {
                GamepadAim();
            }

            if (!hasOnePotion)
                return;

            if (PlayerController.Controls.Action.Throw.IsPressed())
            {
                for (int i = 0; i < pointsCount; i++)
                {
                    points[i].transform.position = PointPosition(i * 0.1f);
                    points[i].SetActive(true);
                }

                if (!points[0].activeSelf)
                {
                    for (int i = 0; i < pointsCount; i++)
                    {
                        points[i].SetActive(true);
                    }
                }
            }

            if (!PlayerController.Controls.Action.Throw.WasReleasedThisFrame()) return;

            for (int i = 0; i < pointsCount; i++)
            {
                points[i].SetActive(false);
            }

            if (potions[selectedPotion] == null || potions[selectedPotion].IsActive) return;

            GameObject projectileGO =
                Instantiate(potions[selectedPotion].Throw(), transform.position, Quaternion.identity);
            potions[selectedPotion] = null;
            projectileGO.GetComponent<Rigidbody2D>().velocity =
                projectileDirection * (launchForce * projectileStrength);
            CheckRecipe();
            audio.PlayOneShot(throwBottle);
            if (onThrow != null)
                onThrow.Invoke(selectedPotion);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Ingredient>())
            {
                canCollect = true;
                canUse = false;
                resourceToCollect = other.GetComponent<Ingredient>();
            }
            else if (other.GetComponent<Telescope>())
            {
                canUse = true;
                canCollect = false;
                telescope = other.GetComponent<Telescope>();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (canCollect && other.gameObject == resourceToCollect.gameObject)
            {
                canCollect = false;
            }
            else if (canUse && other.gameObject == telescope.gameObject)
            {
                canUse = false;
            }
        }

        private void Scroll()
        {
            if (PlayerController.Controls.Other.ScrollPotionDown.WasPressedThisFrame())
            {
                selectedPotion--;
                if (selectedPotion == -1)
                    selectedPotion = 2;
            }

            if (PlayerController.Controls.Other.ScrollPotionUp.WasPressedThisFrame())
            {
                selectedPotion++;
                if (selectedPotion == 3)
                    selectedPotion = 0;
            }

            if (PlayerController.Controls.Other.Potion3.WasPerformedThisFrame()) selectedPotion = 2;
            if (PlayerController.Controls.Other.Potion2.WasPerformedThisFrame()) selectedPotion = 1;
            if (PlayerController.Controls.Other.Potion1.WasPerformedThisFrame()) selectedPotion = 0;

            onSelect?.Invoke(selectedPotion);
        }

        private void Collect()
        {
            if (!canAdd)
                return;

            if (!canCollect) return;

            audio.PlayOneShot(collect);
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
                        potions[i] = ((IPotion) recipe.output);

                        if (onRecipeComplete != null)
                        {
                            onRecipeComplete.Invoke((IPotion) recipe.output, i);
                            audio.PlayOneShot(craft);
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
            audio.PlayOneShot(die);
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

        private Vector2 PointPosition(float t)
        {
            Vector2 currentPosition = (Vector2) transform.position +
                                      projectileDirection * (launchForce * t * projectileStrength) + Physics2D.gravity *
                                      (2.5f * .5f * t * t);
            return currentPosition;
        }

        private void MouseAim()
        {
            Vector2 aimDir =
                (Vector2) cam.ScreenToWorldPoint(PlayerController.Controls.Action.KeyboardAim.ReadValue<Vector2>()) -
                (Vector2) transform.position;
            projectileStrength = launchDistTransformation.Evaluate(aimDir.sqrMagnitude);
            projectileDirection = aimDir.normalized;
        }

        private void GamepadAim()
        {
            Vector2 aimDir = PlayerController.Controls.Action.GamepadAim.ReadValue<Vector2>();
            projectileStrength = aimDir.magnitude;
            projectileDirection = aimDir.normalized;
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