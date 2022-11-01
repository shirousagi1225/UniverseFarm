using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmland : MonoBehaviour
{
    public FarmlandName farmlandName;

    private SpriteRenderer farmlandSR;
    public SpriteRenderer smallIconSR;

    [HideInInspector]public bool isBackpackOpen = false;
    private bool isUnlock;
    private bool isRepair;

    private void Awake()
    {
        farmlandSR = GetComponent<SpriteRenderer>();
    }

    public bool canPlant
    {
        get
        {
            if (!isUnlock || isRepair || transform.childCount>1)
                return false;
            else
                return true;
        }
    }

    public void FarmlandClicked()
    {
        if (!isUnlock)
        {
            Unlock();
            //須加入變換農地狀態方法
        }
        else if (isRepair)
        {
            //突發事件系統相關
        }
    }

    //解鎖方法(未完成)
    public void Unlock()
    {
        //須引用判斷:是否有足夠金額購買方法
        isUnlock = true;
        isRepair = false;
        FarmlandManager.Instance.AddFarmland(farmlandName);
        smallIconSR.gameObject.SetActive(true);
        //測試用,正式刪除
        GetComponent<Collider2D>().enabled = false;
    }

    //修繕方法(未完成)

    public void PlantAction(GameObject mainCanvas)
    {
        isBackpackOpen = !isBackpackOpen;
        if (isBackpackOpen)
        {
            for (int i=0; i<mainCanvas.transform.childCount;i++)
            {
                mainCanvas.transform.GetChild(i).gameObject.SetActive(false);
            }
            mainCanvas.transform.GetChild(8).gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < mainCanvas.transform.childCount; i++)
            {
                mainCanvas.transform.GetChild(i).gameObject.SetActive(true);
            }
            mainCanvas.transform.GetChild(8).gameObject.SetActive(false);
        }
    }

    //須寫變換小圖示狀態方法(未完成)
}
