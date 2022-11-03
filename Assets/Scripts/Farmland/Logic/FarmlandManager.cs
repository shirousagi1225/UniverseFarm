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
    public void CreateCrop(ItemDetails seedDetails, ItemName itemName, GameObject crop, Collider2D farmlandCD, GameObject mainCanvas)
    {
        Instantiate(crop, farmlandCD.gameObject.transform);
        farmlandCD.enabled = false;
        farmlandCD.gameObject.transform.GetChild(1).GetComponent<Crop>().SetCrop(seedDetails, itemName);
        ////�W�[UIManager�ᶷ���,�åB�[�J�P�_�A�a�O�_�S�ش�,�������A�a�ҺشӦb����UI
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
        //�ܴ��A�a���A
        //�A�к޲z(����B��µ)
        //�@�����p���ܡ]�ʤ��B�`�ΡB�I�Ρ^�G�H���v����
        //�A�д��ܡ]�i½�g�B�i�شӡB�i�����^
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

    //�����q��k(������)
    public int Produce(ItemName seedName)
    {
        return cropData.GetCropStateDetails(seedName).produce;
    }
}
