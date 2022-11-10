using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crop : MonoBehaviour
{
    public ItemName seedName;
    public ItemName cropName;

    private bool isInfoBarOpen = false;

    public void CropClicked()
    {
        //測試用須以成長時間判斷是否可收成,正式改為顧客是否購買判斷
        ShowInfoBar();
        Harvest();
    }

    public void SetCrop(ItemDetails seedDetails, ItemName itemName)
    {
        //測試用,正式seedName跟cropName的值對調
        seedName = itemName;
        cropName = seedDetails.itemName;
        //測試用,正式不需要加
        GetComponent<SpriteRenderer>().sprite = seedDetails.itemSprite;
    }

    //作物收成方法(未完成)
    private void Harvest()
    {
        InventoryManager.Instance.AddItem(cropName, seedName, FarmlandManager.Instance.Produce(seedName));
        transform.parent.GetComponent<Collider2D>().enabled = true;
        Destroy(gameObject);
    }

    //狀態資訊欄顯示方法(未完成)
    public void ShowInfoBar()
    {
        //須判斷點選哪個作物,場景上只能顯示點選作物的資訊欄,開啟下個前須先關閉前一個
        //測試用,正式true改為!isInfoBarOpen
        isInfoBarOpen = true;
        if (isInfoBarOpen)
            transform.parent.GetChild(1).GetComponent<CanvasGroup>().alpha=1.0f;
        else
            transform.parent.GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
        transform.parent.GetChild(1).GetComponent<CanvasGroup>().interactable = isInfoBarOpen;
        transform.parent.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = isInfoBarOpen;
        EventHandler.CallUpdateGrowTimeEvent(transform.parent.GetComponent<Farmland>().farmlandName, isInfoBarOpen, 
            transform.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>());
    }
}
