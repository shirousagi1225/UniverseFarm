using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public GameObject panel;
    public Image shopBar;
    public Sprite cropShopSprite;
    public Sprite obsidianShopSprite;
    public GameObject cropShop;
    public GameObject commodityContent;
    public GameObject obsidianShop;
    public GameObject commodity;

    private void OnEnable()
    {
        EventHandler.SetShopUIEvent += OnSetShopUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.SetShopUIEvent -= OnSetShopUIEvent;
    }

    private void OnSetShopUIEvent(ItemDetails seedDetails, ItemDetails itemDetails)
    {
        Instantiate(commodity, commodityContent.transform);
        commodityContent.transform.GetChild(commodityContent.transform.childCount-1).GetComponent<CommoditySlotUI>().SetCommodity(seedDetails, itemDetails);
    }

    public void ShowShopUI()
    {
        //�ҰʤlUI
        EventHandler.CallShowSecUIEvent("SecCanvas", true, false);
        panel.SetActive(true);
    }

    public void ChangeShopType(GameObject shopType)
    {
        if(shopType.name== "CropShop")
        {
            shopBar.sprite = obsidianShopSprite;
            cropShop.SetActive(false);
            obsidianShop.SetActive(true);
        }
        else
        {
            shopBar.sprite = cropShopSprite;
            cropShop.SetActive(true);
            obsidianShop.SetActive(false);
        }
    }

    public void CloseShopUI()
    {
        panel.SetActive(false);
        EventHandler.CallShowSecUIEvent("SecCanvas", false, false);
    }
}
