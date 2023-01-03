using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommoditySlotUI : MonoBehaviour
{
    public ItemName seedName;
    public ItemName itemName;
    public Image commodityImage;
    public Text price;

    private ItemDetails currentSeedDetails;
    public void SetCommodity(ItemDetails seedDetails, ItemDetails itemDetails)
    {
        seedName= seedDetails.itemName;
        itemName = itemDetails.itemName;
        currentSeedDetails = seedDetails;
        commodityImage.sprite = seedDetails.itemSprite;
        commodityImage.SetNativeSize();
        price.text = seedDetails.itemPrice.ToString();
    }

    public void BuyCommodity()
    {
        InventoryManager.Instance.AddSeed(seedName, itemName,1,true);
        EventHandler.CallSetUniversalUIEvent(UniversalUIType.ShopBuy, currentSeedDetails, 1);
    }
}
