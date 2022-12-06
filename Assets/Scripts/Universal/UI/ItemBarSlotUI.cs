using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBarSlotUI : MonoBehaviour
{
    public Image itemImage;
    public Text itemCount;

    public void SetItem(ItemDetails itemDetails, int count)
    {
        itemImage.sprite = itemDetails.itemSprite;
        itemImage.SetNativeSize();
        itemCount.text = count.ToString();
    }
}
