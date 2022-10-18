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
        //�ܴ��A�a���A(�H���v����)
    }

    public void SetCropState()
    {
        //�ܴ��@���ͪ����A(�H�ɶ�����)
    }

    public void AddFarmland(FarmlandName farmlandName)
    {
        if (!farmlandList.Contains(farmlandName))
        {
            farmlandList.Add(farmlandName);
        }
    }

    //���g�����q��k
}
