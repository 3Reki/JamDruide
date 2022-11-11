using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> resourceImages;
    [SerializeField] private List<Image> CraftUI;
    [SerializeField] private Animator craftedPotionUI;
    
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
        PlayerActions.onRecipeComplete += DelayAnimation;
    }

    private void OnDisable()
    {
        PlayerActions.onCollect -= UpdateUI;
        PlayerActions.onRecipeComplete -= DelayAnimation;
    }

    private void UpdateUI(int resourceIndex, CraftsList.Resources resourceType)
    {
        CraftUI[resourceIndex].sprite = resourcesImages[resourceType];
        CraftUI[resourceIndex].GetComponent<Animator>().Play("UIResourceGet");
    }

    private IEnumerator DelayAnimation(int index)
    {
        yield return animDelay;
        CraftUI[index].GetComponent<Animator>().Play("UIResourceUse");
        craftedPotionUI.Play("UICraftedPotion");
    }
}
