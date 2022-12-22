using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmlandManager : Singleton<FarmlandManager>,ISaveable
{
    public FarmlandDataList_SO farmlandData;
    public FarmlandStateDataList_SO farmlandStateData;
    public CropDataList_SO cropData;

    [SerializeField] private List<FarmlandName> farmlandList = new List<FarmlandName>();

    private void OnEnable()
    {
        //註冊保存數據
        ISaveable saveable = this;
        saveable.SaveableRegister();

        EventHandler.UpdateFarmlandStateEvent += OnUpdateFarmlandStateEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateFarmlandStateEvent -= OnUpdateFarmlandStateEvent;
    }

    //變換農地狀態事件(未完成)
    private void OnUpdateFarmlandStateEvent(Farmland currentFarm)
    {
        //農田管理(解鎖、修繕)
        if (currentFarm.canPlant)
        {
            //農田提示(可種植)
            currentFarm.transform.GetChild(0).gameObject.SetActive(true);
            currentFarm.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = farmlandStateData.GetFarmlandStateDetails(FarmlandState.Plant).farmlandStateSprite;
        }
        else if (currentFarm.transform.GetChild(2).GetComponent<Crop>().growthStage == 3)
        {
            //農田提示(可收成)
            currentFarm.transform.GetChild(0).gameObject.SetActive(true);
            currentFarm.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = farmlandStateData.GetFarmlandStateDetails(FarmlandState.Harvest).farmlandStateSprite;
            //Debug.Log(currentFarm.transform.GetChild(2).GetComponent<Crop>().growthStage);
        }
        else if(currentFarm.transform.GetChild(2).GetComponent<Crop>().growthStage == 4)
        {
            //農田提示(可翻土)
            currentFarm.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = farmlandStateData.GetFarmlandStateDetails(FarmlandState.Dig).farmlandStateSprite;
            EventHandler.CallUpdateHintUIEvent(currentFarm, currentFarm.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);
        }
        else if (AlgorithmManager.Instance.TriggerAbnormalState())
        {
            //作物狀況提示（缺水、害蟲、施肥）：以機率控制
            currentFarm.transform.GetChild(0).gameObject.SetActive(true);
            currentFarm.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = farmlandStateData.GetFarmlandStateDetails(AlgorithmManager.Instance.ChooseAbnormalState()).farmlandStateSprite;
            EventHandler.CallUpdateHintUIEvent(currentFarm, currentFarm.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite);
            Debug.Log("異常狀態");
        }
        //Debug.Log(currentFarm.transform.GetChild(0).gameObject.activeInHierarchy);
    }

    /*public FarmlandDetails GetFarmlandInfo(FarmlandName farmlandName)
    {
        return farmlandData.GetFarmlandDetails(farmlandName);
    }*/

    //生成作物方法(未完成)
    public void CreateCrop(ItemDetails seedDetails, ItemName itemName, GameObject crop, Collider2D farmlandCD)
    {
        int canPlantCount=0;

        farmlandCD.transform.GetChild(0).gameObject.SetActive(false);
        Instantiate(crop, farmlandCD.gameObject.transform);
        farmlandCD.enabled = false;
        farmlandCD.gameObject.transform.GetChild(2).GetComponent<Crop>().SetCrop(seedDetails, itemName);
        //測試用,正式itemName改為seedDetails(已修改)
        SetCropStageTime(seedDetails.itemName, cropData.GetCropStateDetails(seedDetails.itemName));
        //測試用,正式itemName改為seedDetails(已修改)
        EventHandler.CallSetGrowTimeEvent(farmlandCD.GetComponent<Farmland>().farmlandName, cropData.GetCropStateDetails(seedDetails.itemName));
        //判斷農地是否沒種植,等全部農地皆種植在關閉UI
        foreach (var farmland in FindObjectsOfType<Farmland>())
        {
            if (farmland.canPlant)
                canPlantCount++;
        }
        if(canPlantCount==0)
            UIManager.Instance.ShowBackpackBarUI();
        InventoryManager.Instance.ReduceItem(seedDetails.itemName, 1);
    }

    //設定作物各成長階段時間方法
    private void SetCropStageTime(ItemName seedName, CropStateDetails cropStateDetails)
    {
        if (cropData.GetCropDetails(seedName, CropState.Sowing).cropStageTime!= new TimeSpan(0,cropStateDetails.growTimeMin, cropStateDetails.growTimeSec)) 
        {
            int stageTime = ((cropStateDetails.growTimeMin * 60 + cropStateDetails.growTimeSec) / 3);

            cropData.GetCropDetails(seedName, CropState.Sowing).cropStageTime = new TimeSpan(0,cropStateDetails.growTimeMin, cropStateDetails.growTimeSec);
            if (stageTime * 2 >= 60)
                cropData.GetCropDetails(seedName, CropState.Seeding).cropStageTime = new TimeSpan(0,(stageTime * 2) / 60, (stageTime * 2) % 60);
            else
                cropData.GetCropDetails(seedName, CropState.Seeding).cropStageTime = new TimeSpan(0,0, stageTime * 2);
            //Debug.Log(cropData.GetCropDetails(seedName, CropState.Seeding).cropStageTime);
            if (stageTime >= 60)
                cropData.GetCropDetails(seedName, CropState.Growing).cropStageTime = new TimeSpan(0,stageTime / 60, stageTime % 60);
            else
                cropData.GetCropDetails(seedName, CropState.Growing).cropStageTime = new TimeSpan(0,0, stageTime);
            //Debug.Log(cropData.GetCropDetails(seedName, CropState.Growing).cropStageTime);
            cropData.GetCropDetails(seedName, CropState.Ageing).cropStageTime = new TimeSpan(0, 0, 0);
        }
    }

    //變換作物生長狀態(以時間控制)
    public void SetCropState(SpriteRenderer currentCropSR, ItemName seedName, TimeSpan currentGrowTime)
    {
        //Debug.Log(seedName);
        if (currentGrowTime <= cropData.GetCropDetails(seedName, CropState.Ageing).cropStageTime && currentCropSR.GetComponentInParent<Crop>().growthStage < 3)
        {
            currentCropSR.sprite = cropData.GetCropDetails(seedName, CropState.Ageing).cropStateSprite;
            currentCropSR.GetComponentInParent<Crop>().growthStage = 3;
            //Debug.Log(currentCropSR.GetComponentInParent<Crop>().growthStage);
        }
        else if (currentGrowTime <= cropData.GetCropDetails(seedName, CropState.Growing).cropStageTime && currentCropSR.GetComponentInParent<Crop>().growthStage < 2)
        {
            currentCropSR.sprite = cropData.GetCropDetails(seedName, CropState.Growing).cropStateSprite;
            currentCropSR.GetComponentInParent<Crop>().growthStage = 2;
            //Debug.Log(currentCropSR.GetComponentInParent<Crop>().growthStage);
        }
        else if (currentGrowTime <= cropData.GetCropDetails(seedName, CropState.Seeding).cropStageTime && currentCropSR.GetComponentInParent<Crop>().growthStage < 1)
        {
            currentCropSR.sprite = cropData.GetCropDetails(seedName, CropState.Seeding).cropStateSprite;
            currentCropSR.GetComponentInParent<Crop>().growthStage = 1;
            //Debug.Log(currentCropSR.GetComponentInParent<Crop>().growthStage);
        }
    }

    public void AddFarmland(FarmlandName farmlandName)
    {
        if (!farmlandList.Contains(farmlandName))
        {
            farmlandList.Add(farmlandName);
        }
    }

    //當次產量方法：受顧客購買數量影響
    public int Produce(ItemName seedName)
    {
        return cropData.GetCropStateDetails(seedName).produce;
    }

    public SaveData GenerateSaveData()
    {
        SaveData saveData = new SaveData();
        ///saveData.farmlandList = farmlandList;
        return saveData;
    }

    public void RestoreGameData(SaveData saveData)
    {
        //farmlandList=saveData.farmlandList;
    }
}
