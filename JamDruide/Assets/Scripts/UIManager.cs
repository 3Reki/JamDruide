using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using Potions;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> resourceImages;
    [SerializeField] private List<Image> CraftUI;
    [SerializeField] private Animator craftedPotionUI;
    [SerializeField] private Image craftedPotionImage;
    [SerializeField] private List<Image> potionsInventory;
    [SerializeField] private GameObject selectedPotion;
    [SerializeField] private Image doubleJumpSlider;
    [SerializeField] private GameObject doubleJumpGameObject;
    [SerializeField] private Image speedSlider;
    [SerializeField] private GameObject speedGameObject;

    private Dictionary<CraftsList.Resources, Sprite> resourcesImages;
    private readonly WaitForSeconds animDelay = new(0.5f);

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


    private void OnEnable()
    {
        PlayerActions.onCollect += UpdateUI;
        PlayerActions.onRecipeComplete += OnRecipeComplete;
        PlayerActions.onThrow += RemoveInventory;
        PlayerActions.onSelect += SelectPotion;
        PauseMenu.OnPause.AddListener(() => enabled = false);
        PauseMenu.OnResume.AddListener(() => enabled = true);
        SpeedPotion.onDrink += SpeedSlider;
        DoubleJumpPotion.onDrink += DoubleJumpSlider;
    }

    

    private void OnDisable()
    {
        PlayerActions.onCollect -= UpdateUI;
        PlayerActions.onRecipeComplete -= OnRecipeComplete;
        PlayerActions.onThrow -= RemoveInventory;
        PlayerActions.onSelect -= SelectPotion;
        PauseMenu.OnPause.RemoveListener(() => enabled = false);
        PauseMenu.OnResume.RemoveListener(() => enabled = true);
        SpeedPotion.onDrink -= SpeedSlider;
        DoubleJumpPotion.onDrink -= DoubleJumpSlider;
    }

    private void UpdateUI(int resourceIndex, CraftsList.Resources resourceType)
    {
        CraftUI[resourceIndex].sprite = resourcesImages[resourceType];
        CraftUI[resourceIndex].enabled = true;
        AnimateGet(CraftUI[resourceIndex].transform);
    }

    private void OnRecipeComplete(IPotion potion, int slot)
    {
        craftedPotionImage.sprite = potion.Sprite;

        StartCoroutine(DelayAnimation(slot));
    }

    private IEnumerator DelayAnimation(int slot)
    {
        yield return animDelay;
        
        for (int i = 0; i < 2; i++)
        {
            AnimateUse(CraftUI[i].transform);
        }
        
        FillInventory(slot);
        craftedPotionUI.Play("UICraftedPotion");
    }

    private void FillInventory(int slot)
    {
        potionsInventory[slot].sprite = craftedPotionImage.sprite;
        potionsInventory[slot].enabled = true;
        AnimateGet(potionsInventory[slot].transform);
    }

    private void RemoveInventory(int slot)
    {
        StartCoroutine(DelayPotionAnimation(slot));
    }
    
    private IEnumerator DelayPotionAnimation(int index)
    {
        AnimateUse(potionsInventory[index].transform);
        yield return animDelay;
        potionsInventory[index].sprite = null;
        potionsInventory[index].enabled = false;
    }

    private void SelectPotion(int select)
    {
        if (select == 2)
            selectedPotion.transform.position = potionsInventory[2].transform.position;
        else if(select == 1)
            selectedPotion.transform.position = potionsInventory[1].transform.position;
        else
            selectedPotion.transform.position = potionsInventory[0].transform.position;
    }
    
    private void DoubleJumpSlider(float duration)
    {
        HandleSlider(doubleJumpSlider, doubleJumpGameObject, duration);
    }
    
    private void SpeedSlider(float duration)
    {
        HandleSlider(speedSlider, speedGameObject, duration);
    }

    private static void HandleSlider(Image slider, GameObject sliderGameObject, float duration)
    {
        sliderGameObject.SetActive(true);
        slider.fillAmount = 1;
        slider.DOFillAmount(0, duration).onComplete = () => sliderGameObject.SetActive(false);
    }

    private static void AnimateUse(Transform imageTransform)
    {
        imageTransform.DOKill();
        imageTransform.localScale = Vector3.one;
        imageTransform.DOScale(1.3f, 5f / 60f).onComplete = () => imageTransform.DOScale(0, 1f / 3f);
    }
    
    private static void AnimateGet(Transform imageTransform)
    {
        imageTransform.DOKill();
        imageTransform.localScale = Vector3.zero;
        imageTransform.DOScale(1.3f, 17f / 60f).onComplete = () => imageTransform.DOScale(1, 1f / 20f);
    }
}
