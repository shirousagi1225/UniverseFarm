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
    public void CreateCrop(ItemDetails seedDetails, ItemName itemName, GameObject crop, Collider2D farmlandCD, GameObject mainCanvas)
    {
        Instantiate(crop, farmlandCD.gameObject.transform);
        farmlandCD.enabled = false;
        farmlandCD.gameObject.transform.GetChild(1).GetComponent<Crop>().SetCrop(seedDetails, itemName);
        ////增加UIManager後須更改,並且加入判斷農地是否沒種植,等全部農地皆種植在關閉UI
        farmlandCD.GetComponent<Farmland>().isBackpackOpen = !farmlandCD.GetComponent<Farmland>().isBackpackOpen;
        for (int i = 0; i < mainCanvas.transform.childCount; i++)
        {
            mainCanvas.transform.GetChild(i).gameObject.SetActive(true);
        }
        mainCanvas.transform.GetChild(8).gameObject.SetActive(false);
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
        return cropData.GetCropStateDetails(seedName).produce;
    }
}
