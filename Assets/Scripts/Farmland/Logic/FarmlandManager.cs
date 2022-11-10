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

    //�ͦ��@����k(������)
    public void CreateCrop(ItemDetails seedDetails, ItemName itemName, GameObject crop, Collider2D farmlandCD)
    {
        int canPlantCount=0;

        Instantiate(crop, farmlandCD.gameObject.transform);
        //���ե�,����itemName�אּseedDetails
        SetCropStageTime(itemName, cropData.GetCropStateDetails(itemName));
        //���ե�,����itemName�אּseedDetails
        EventHandler.CallSetGrowTimeEvent(farmlandCD.GetComponent<Farmland>().farmlandName, cropData.GetCropStateDetails(itemName));
        farmlandCD.enabled = false;
        farmlandCD.gameObject.transform.GetChild(2).GetComponent<Crop>().SetCrop(seedDetails, itemName);
        //�P�_�A�a�O�_�S�ش�,�������A�a�ҺشӦb����UI
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
        //�ܴ��A�a���A
        //�A�к޲z(����B��µ)
        //�@�����p���ܡ]�ʤ��B�`�ΡB�I�Ρ^�G�H���v����
        //�A�д��ܡ]�i½�g�B�i�شӡB�i�����^
    }

    //�]�w�@���U�������q�ɶ���k
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

    //�ܴ��@���ͪ����A(�H�ɶ�����)
    public void SetCropState(SpriteRenderer currentCropSR, ItemName seedName, TimeSpan currentGrowTime)
    {
        //Debug.Log(seedName);
        //�i�ﵽ�G���C���Ƴ]�m����Ϥ�,�u�b�̪�F������]�m
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

    //�����q��k(������)
    public int Produce(ItemName seedName)
    {
        //�����U���ʶR��k����,�P�_�Ѿl�h�ֲ��q
        return cropData.GetCropStateDetails(seedName).produce;
    }
}
