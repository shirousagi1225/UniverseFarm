using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public Image itemImage;

    private ItemDetails currentItem;
    private bool isSelected;

    public void SetItem(ItemDetails itemDetails)
    {
        //須寫抓取持有物數量方式
        currentItem=itemDetails;
        itemImage.sprite=itemDetails.itemSprite;
        itemImage.SetNativeSize();
    }

    public void SetEmpty()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
