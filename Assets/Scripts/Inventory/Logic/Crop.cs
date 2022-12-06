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

    //測試用,正式3改為0
    private int _growthStage=3;
    private bool isInfoBarOpen = false;

    public int growthStage
    {
        get
        {
            return _growthStage;
        }

        set
        {
            _growthStage=value;
        }
    }

    public void CropClicked()
    {
        //測試用須以成長階段判斷是否可收成,正式改為顧客是否購買判斷(已修改)
        StartCoroutine(ShowInfoBar());
    }

    public void SetCrop(ItemDetails seedDetails, ItemName itemName)
    {
        //測試用,正式seedName跟cropName的值對調(已修改)
        seedName = seedDetails.itemName;
        cropName = itemName;
        //測試用,正式不需要加
        _growthStage = 0;
    }

    //作物收成方法(未完成)
    public IEnumerator Harvest()
    {
        if (isInfoBarOpen)
            yield return ShowInfoBar();
        else
            yield return new WaitForSeconds(0);

        EventHandler.CallUpdateFarmlandStateEvent(transform.parent.GetComponent<Farmland>());
        transform.parent.GetComponent<Collider2D>().enabled = true;
        Destroy(gameObject);
    }

    //狀態資訊欄顯示方法(未完成)
    private IEnumerator ShowInfoBar()
    {
        //須判斷點選哪個作物,場景上只能顯示點選作物的資訊欄,開啟下個前須先關閉前一個
        isInfoBarOpen = !isInfoBarOpen;

        EventHandler.CallUpdateGrowTimeEvent(transform.parent.GetComponent<Farmland>().farmlandName, isInfoBarOpen,
            transform.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>());

        yield return new WaitForSeconds(1);

        if (isInfoBarOpen)
            transform.parent.GetChild(1).GetComponent<CanvasGroup>().alpha=1.0f;
        else
            transform.parent.GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
        transform.parent.GetChild(1).GetComponent<CanvasGroup>().interactable = isInfoBarOpen;
        transform.parent.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = isInfoBarOpen;
    }
}
