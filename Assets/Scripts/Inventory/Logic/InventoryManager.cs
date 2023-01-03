using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>,ISaveable
{
    public ItemDataList_SO itemData;
    public ItemDataList_SO seedData;

    [SerializeField] private List<ItemName> itemList=new List<ItemName>();
    [SerializeField] private List<ItemName> seedList = new List<ItemName>();

    private void Start()
    {
        //���U�O�s�ƾ�
        ISaveable saveable = this;
        saveable.SaveableRegister();

        EventHandler.CallUpdateMoneyUIEvent(itemData.GetItemDetails(ItemName.StarStone));
        EventHandler.CallUpdateMoneyUIEvent(itemData.GetItemDetails(ItemName.Obsidian));
    }

    //seedName�ѼƩ���ըϥ�,�����N�g�b�W�[�ؤl��k(�w�ק�)
    public void AddItem(ItemName itemName,int soldCount, int itemCount)
    {
        if (!itemList.Contains(itemName))
        {
            if (itemName.ToString()== "StarStone"|| itemName.ToString() == "Obsidian") 
            {
                itemData.GetItemDetails(itemName).itemCount += itemCount;
                EventHandler.CallUpdateMoneyUIEvent(itemData.GetItemDetails(itemName));
            }
            else
            {
                itemList.Add(itemName);
                EventHandler.CallUpdateCropPokedexEvent(itemData.GetItemDetails(itemName));
                itemData.GetItemDetails(itemName).itemCount += itemCount;
                EventHandler.CallUpdateInventoryUIEvent(itemData.GetItemDetails(itemName), ItemName.None, itemList.Count-1,true,"Item");
                //�z�L���j�P�ɶi��f���ƶq��s
                AddItem(ItemName.StarStone, 0, itemData.GetItemDetails(itemName).itemPrice * soldCount);
            }
        }
        else
        {
            itemData.GetItemDetails(itemName).itemCount += itemCount;
            EventHandler.CallUpdateInventoryUIEvent(itemData.GetItemDetails(itemName), ItemName.None, itemList.FindIndex(i => i == itemName), false, "Item");
            //�z�L���j�P�ɶi��f���ƶq��s
            AddItem(ItemName.StarStone, 0, itemData.GetItemDetails(itemName).itemPrice * soldCount);
        }

        //�ݫ�ҳq��UI�������w�q
        //���ѨM��ܸ�q��UI�P�ɥX�{���D
        EventHandler.CallSetUniversalUIEvent(UniversalUIType.CustomerSell, itemData.GetItemDetails(itemName), itemCount);
    }

    //�W�[�ؤl��k(������)
    public void AddSeed(ItemName seedName, ItemName itemName, int seedCount,bool isShopBuy)
    {
        if (!seedList.Contains(seedName))
        {
            seedList.Add(seedName);
            seedData.GetItemDetails(seedName).itemCount += seedCount;
            EventHandler.CallUpdateInventoryUIEvent(seedData.GetItemDetails(seedName), itemName, seedList.Count - 1, true, "Seed");
        }
        else
        {
            seedData.GetItemDetails(seedName).itemCount += seedCount;
            EventHandler.CallUpdateInventoryUIEvent(seedData.GetItemDetails(seedName), itemName, seedList.FindIndex(i => i == seedName), false, "Seed");
        }

        //�P�_�ؤl�]���o�覡
        if (isShopBuy)
            ReduceItem(ItemName.StarStone, seedData.GetItemDetails(seedName).itemPrice* seedCount);

        //���ե�,�����R��
        EventHandler.CallSetUniversalUIEvent(UniversalUIType.Confirm, seedData.GetItemDetails(seedName), seedCount);
    }

    //��֫����q��k(������)
    public void ReduceItem(ItemName itemName, int itemCount)
    {
        if (itemName.ToString() == "StarStone" || itemName.ToString() == "Obsidian")
        {
            if (itemCount <= itemData.GetItemDetails(itemName).itemCount)
            {
                itemData.GetItemDetails(itemName).itemCount -= itemCount;
                EventHandler.CallUpdateMoneyUIEvent(itemData.GetItemDetails(itemName));
            }
            else
            {
                //�����ƶq�i���L�k�ʶR
            }
        }
        else if(itemList.Contains(itemName))
        {
            if (itemCount <= itemData.GetItemDetails(itemName).itemCount)
            {
                itemData.GetItemDetails(itemName).itemCount -= itemCount;
                if (itemData.GetItemDetails(itemName).itemCount<=0)
                {
                    EventHandler.CallUpdateInventoryUIEvent(null, ItemName.None, itemList.FindIndex(i => i == itemName), false, "Item");
                    itemList.Remove(itemName);
                }
                else
                    EventHandler.CallUpdateInventoryUIEvent(itemData.GetItemDetails(itemName), ItemName.None, itemList.FindIndex(i => i == itemName), false, "Item");
            }
            else
            {
                //�����ƶq�i���L�k�X��
            }
        }else if (seedList.Contains(itemName))
        {
            if (itemCount <= seedData.GetItemDetails(itemName).itemCount)
            {
                seedData.GetItemDetails(itemName).itemCount -= itemCount;
                if (seedData.GetItemDetails(itemName).itemCount<=0)
                {
                    EventHandler.CallUpdateInventoryUIEvent(null, ItemName.None, seedList.FindIndex(i => i == itemName), false, "Seed");
                    seedList.Remove(itemName);
                }
                else
                    EventHandler.CallUpdateInventoryUIEvent(seedData.GetItemDetails(itemName), ItemName.None, seedList.FindIndex(i => i == itemName), false,"Seed");
            }
        }
    }

    //����ؤl��k
    public void UnlockSeed(ItemName seedName)
    {
        EventHandler.CallSetShopUIEvent(seedData.GetItemDetails(seedName), itemData.GetItemDetails(seedData.GetItemDetails(seedName).cropName));
        Debug.Log(seedName);
    }

    public SaveData GenerateSaveData()
    {
        SaveData saveData = new SaveData();
        saveData.coinCountDict.Add("StarStone".ToString(), itemData.GetItemDetails(ItemName.StarStone).itemCount);
        saveData.coinCountDict.Add("Obsidian".ToString(), itemData.GetItemDetails(ItemName.Obsidian).itemCount);
        saveData.itemList = itemList;
        saveData.seedList = seedList;
        foreach (var item in itemList)
            saveData.itemCountDict.Add(item.ToString(), itemData.GetItemDetails(item).itemCount);
        foreach (var seed in seedList)
            saveData.seedCountDict.Add(seed.ToString(), seedData.GetItemDetails(seed).itemCount);

        return saveData;
    }

    public void RestoreGameData(SaveData saveData)
    {
        itemData.GetItemDetails(ItemName.StarStone).itemCount = saveData.coinCountDict["StarStone"];
        itemData.GetItemDetails(ItemName.Obsidian).itemCount = saveData.coinCountDict["Obsidian"];
        itemList = saveData.itemList;
        seedList = saveData.seedList;
        foreach (var item in saveData.itemList)
            itemData.GetItemDetails(item).itemCount = saveData.itemCountDict[item.ToString()];
        foreach (var seed in saveData.seedList)
            seedData.GetItemDetails(seed).itemCount = saveData.seedCountDict[seed.ToString()];

        //�٭�f���ƶq
        EventHandler.CallUpdateMoneyUIEvent(itemData.GetItemDetails(ItemName.StarStone));
        EventHandler.CallUpdateMoneyUIEvent(itemData.GetItemDetails(ItemName.Obsidian));

        //�٭�@�����~��
        int itemIndex =0;
        foreach (var item in itemList)
        {
            EventHandler.CallUpdateInventoryUIEvent(itemData.GetItemDetails(item), ItemName.None, itemIndex, true, "Item");
            itemIndex++;
        }

        //�٭�ؤl���~��
        int seedIndex = 0;
        foreach (var seed in seedList)
        {
            EventHandler.CallUpdateInventoryUIEvent(seedData.GetItemDetails(seed), seedData.GetItemDetails(seed).cropName, seedIndex, true, "Seed");
            seedIndex++;
        }
    }
}
