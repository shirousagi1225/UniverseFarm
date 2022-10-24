using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public ItemName itemName;
    public ItemName seedName;

    public void CropClicked()
    {
        Harvest();
    }

    //作物收成方法(未完成)
    private void Harvest()
    {
        InventoryManager.Instance.AddItem(itemName, FarmlandManager.Instance.Produce(seedName));
        Destroy(gameObject);
    }

    //狀態資訊欄顯示方法(未完成)
}
