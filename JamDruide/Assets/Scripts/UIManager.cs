using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] GameObject selectedPotion;

    public static GameObject[] sliderObj = new GameObject[2];

    [SerializeField] Transform sliderContainer;

    private Dictionary<CraftsList.Resources, Sprite> resourcesImages;
    private WaitForSeconds animDelay = new(0.5f);

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
        PlayerActions.onDeath += ResetInventoryAfterDeath;
        PauseMenu.OnPause.AddListener(() => enabled = false);
        PauseMenu.OnResume.AddListener(() => enabled = true);
    }


    private void OnDisable()
    {
        PlayerActions.onCollect -= UpdateUI;
        PlayerActions.onRecipeComplete -= OnRecipeComplete;
        PlayerActions.onThrow -= RemoveInventory;
        PlayerActions.onSelect -= SelectPotion;
        PauseMenu.OnPause.RemoveListener(() => enabled = false);
        PauseMenu.OnResume.RemoveListener(() => enabled = true);
    }

    private void UpdateUI(int resourceIndex, CraftsList.Resources resourceType)
    {
        CraftUI[resourceIndex].sprite = resourcesImages[resourceType];
        CraftUI[resourceIndex].GetComponent<Animator>().Play("UIResourceGet");
    }

    private void OnRecipeComplete(IPotion potion, int slot)
    {
        craftedPotionImage.sprite = potion.Sprite;
        for (int i = 0; i < 2; i++)
        {
            StartCoroutine(DelayAnimation(i));
        }
        FillInventory(slot);
    }

    private IEnumerator DelayAnimation(int index)
    {
        yield return animDelay;
        CraftUI[index].GetComponent<Animator>().Play("UIResourceUse");
        craftedPotionUI.Play("UICraftedPotion");
    }
    
    private void ResetInventoryAfterDeath()
    {
        for (int i = 0; i < 2; i++)
        {
             CraftUI[i].GetComponent<Animator>().Play("UIResourceUse");
        }
    }

    void FillInventory(int slot)
    {
        potionsInventory[slot].sprite = craftedPotionImage.sprite;
        potionsInventory[slot].GetComponent<Animator>().Play("UIResourceGet");
    }
    void RemoveInventory(int slot)
    {
        StartCoroutine(DelayPotionAnimation(slot));
    }
    private IEnumerator DelayPotionAnimation(int index)
    {
        potionsInventory[index].GetComponent<Animator>().Play("UIResourceUse");
        yield return animDelay;
        potionsInventory[index].sprite = null;
    }
    void SelectPotion(int select)
    {
        if (select == 2)
            selectedPotion.transform.position = potionsInventory[2].transform.position;
        else if(select == 1)
            selectedPotion.transform.position = potionsInventory[1].transform.position;
        else
            selectedPotion.transform.position = potionsInventory[0].transform.position;
    }
}
