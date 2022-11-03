using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public ItemName seedName;
    public ItemName cropName;

    public void CropClicked()
    {
        Harvest();
    }

    public void SetCrop(ItemDetails seedDetails, ItemName itemName)
    {
        //測試用,正式seedName跟cropName的值對調
        seedName = itemName;
        cropName = seedDetails.itemName;
        GetComponent<SpriteRenderer>().sprite = seedDetails.itemSprite;
    }

    //作物收成方法(未完成)
    private void Harvest()
    {
        InventoryManager.Instance.AddItem(cropName, seedName, FarmlandManager.Instance.Produce(seedName));
        Destroy(gameObject);
        transform.parent.GetComponent<Collider2D>().enabled = true;
    }

    //狀態資訊欄顯示方法(未完成)
}
