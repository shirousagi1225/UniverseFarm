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
        SetCropStageTime(itemName, cropData.GetCropStateDetails(itemName));
        //測試用,正式itemName改為seedDetails
        EventHandler.CallSetGrowTimeEvent(farmlandCD.GetComponent<Farmland>().farmlandName, cropData.GetCropStateDetails(itemName));
        farmlandCD.enabled = false;
        farmlandCD.gameObject.transform.GetChild(2).GetComponent<Crop>().SetCrop(seedDetails, itemName);
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

    //設定作物各成長階段時間方法
    private void SetCropStageTime(ItemName seedName, CropStateDetails cropStateDetails)
    {
        if (cropData.GetCropDetails(seedName, CropState.Sowing).cropStageTime!= new TimeSpan(cropStateDetails.growTimeHr, cropStateDetails.growTimeMin, 0)) 
        {
            int stageTime = ((cropStateDetails.growTimeHr * 60 + cropStateDetails.growTimeMin) / 3);

            cropData.GetCropDetails(seedName, CropState.Sowing).cropStageTime = new TimeSpan(cropStateDetails.growTimeHr, cropStateDetails.growTimeMin, 0);
            if (stageTime * 2 >= 60)
                cropData.GetCropDetails(seedName, CropState.Seeding).cropStageTime = new TimeSpan((stageTime * 2) / 60, (stageTime * 2) % 60, 0);
            else
                cropData.GetCropDetails(seedName, CropState.Seeding).cropStageTime = new TimeSpan(0, stageTime * 2, 0);
            //Debug.Log(cropData.GetCropDetails(seedName, CropState.Seeding).cropStageTime);
            if (stageTime >= 60)
                cropData.GetCropDetails(seedName, CropState.Growing).cropStageTime = new TimeSpan(stageTime / 60, stageTime % 60, 0);
            else
                cropData.GetCropDetails(seedName, CropState.Growing).cropStageTime = new TimeSpan(0, stageTime, 0);
            //Debug.Log(cropData.GetCropDetails(seedName, CropState.Growing).cropStageTime);
            cropData.GetCropDetails(seedName, CropState.Ageing).cropStageTime = new TimeSpan(0, 0, 0);
        }
    }

    //變換作物生長狀態(以時間控制)
    public void SetCropState(SpriteRenderer currentCropSR, ItemName seedName, TimeSpan currentGrowTime)
    {
        //Debug.Log(seedName);
        //可改善：不每秒重複設置物件圖片,只在最初達成條件設置
        if (currentGrowTime <= cropData.GetCropDetails(seedName, CropState.Ageing).cropStageTime)
            currentCropSR.sprite = cropData.GetCropDetails(seedName, CropState.Ageing).cropStateSprite;
        else if(currentGrowTime <= cropData.GetCropDetails(seedName, CropState.Growing).cropStageTime)
            currentCropSR.sprite = cropData.GetCropDetails(seedName, CropState.Growing).cropStateSprite;
        else if(currentGrowTime <= cropData.GetCropDetails(seedName, CropState.Seeding).cropStageTime)
            currentCropSR.sprite = cropData.GetCropDetails(seedName, CropState.Seeding).cropStateSprite;
        else
            currentCropSR.sprite = cropData.GetCropDetails(seedName, CropState.Sowing).cropStateSprite;
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
