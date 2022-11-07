using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmlandManager : Singleton<FarmlandManager>
{
    public FarmlandDataList_SO farmlandData;
    public FarmlandStateDataList_SO farmlandStateData;
    public CropDataList_SO cropData;

    [SerializeField] private List<FarmlandName> farmlandList = new List<FarmlandName>();

    public FarmlandDetails GetFarmlandInfo(FarmlandName farmlandName)
    {
        return farmlandData.GetFarmlandDetails(farmlandName);
    }

    //生成作物方法(未完成)
    public void CreateCrop(ItemDetails seedDetails, ItemName itemName, GameObject crop, Collider2D farmlandCD)
    {
        int canPlantCount=0;

        Instantiate(crop, farmlandCD.gameObject.transform);
        //測試用,正式itemName改為seedDetails
        EventHandler.CallSetGrowTimeEvent(farmlandCD.GetComponent<Farmland>().farmlandName, cropData.GetCropStateDetails(itemName), DateTime.Now);
        farmlandCD.enabled = false;
        farmlandCD.gameObject.transform.GetChild(1).GetComponent<Crop>().SetCrop(seedDetails, itemName);
        //判斷農地是否沒種植,等全部農地皆種植在關閉UI
        foreach (var farmland in FindObjectsOfType<Farmland>())
        {
            if (farmland.canPlant)
                canPlantCount++;
        }
        if(canPlantCount==0)
            UIManager.Instance.ShowSecUI();
        InventoryManager.Instance.ReduceItem(seedDetails.itemName, 1);
    }

    public void SetFarmlandState(FarmlandName farmlandName)
    {
        //變換農地狀態
        //農田管理(解鎖、修繕)
        //作物狀況提示（缺水、害蟲、施肥）：以機率控制
        //農田提示（可翻土、可種植、可收成）
    }

    public void SetCropState()
    {
        //變換作物生長狀態(以時間控制)
    }

    public void AddFarmland(FarmlandName farmlandName)
    {
        if (!farmlandList.Contains(farmlandName))
        {
            farmlandList.Add(farmlandName);
        }
    }

    //當次產量方法(未完成)
    public int Produce(ItemName seedName)
    {
        //須等顧客購買方法完成,判斷剩餘多少產量
        return cropData.GetCropStateDetails(seedName).produce;
    }
}
