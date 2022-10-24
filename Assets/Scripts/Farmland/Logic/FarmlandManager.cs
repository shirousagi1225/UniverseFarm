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

    public void SetSeed(FarmlandName farmlandName, ItemName seedName)
    {
        farmlandData.GetFarmlandDetails(farmlandName).seedName = seedName;
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
        return cropData.GetCropStateDetails(seedName).produce;
    }
}
