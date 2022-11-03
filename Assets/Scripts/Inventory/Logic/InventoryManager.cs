using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public ItemDataList_SO itemData;
    public ItemDataList_SO seedData;

    [SerializeField] private List<ItemName> itemList=new List<ItemName>();
    [SerializeField] private List<ItemName> seedList = new List<ItemName>();

    //seedName�ѼƩ���ըϥ�,�����N�g�b�W�[�ؤl��k
    public void AddItem(ItemName itemName, ItemName seedName, int itemCount)
    {
        if (!itemList.Contains(itemName))
        {
            if (itemName.ToString()== "StarStone"|| itemName.ToString() == "Obsidian") 
            {
                //���g�f��UI�M�ƥ�(������)
                itemData.GetItemDetails(itemName).itemCount += itemCount;
                EventHandler.CallUpdateMoneyUIEvent(itemData.GetItemDetails(itemName));
            }
            else
            {
                itemList.Add(itemName);
                itemData.GetItemDetails(itemName).itemCount += itemCount;
                EventHandler.CallUpdateInventoryUIEvent(itemData.GetItemDetails(itemName), seedName, itemList.Count-1,true,"Item");
            }
        }
        else
        {
            itemData.GetItemDetails(itemName).itemCount += itemCount;
            EventHandler.CallUpdateInventoryUIEvent(itemData.GetItemDetails(itemName), seedName, itemList.FindIndex(i => i == itemName), false, "Item");
        }
    }

    //�W�[�ؤl��k(������)
    public void AddSeed(ItemName seedName, int seedCount)
    {
        if (!seedList.Contains(seedName))
        {
            seedList.Add(seedName);
            seedData.GetItemDetails(seedName).itemCount += seedCount;
        }
        else
        {
            seedData.GetItemDetails(seedName).itemCount += seedCount;
        }
    }

    //��֫����q��k(������)
    public void ReduceItem(ItemName itemName, int itemCount)
    {
        if (itemName.ToString() == "StarStone" || itemName.ToString() == "Obsidian")
        {
            //���Ұ��ʶRUI
            //���[�J�P�_:�O�_���������B�ʶR
            if (itemCount <= itemData.GetItemDetails(itemName).itemCount)
            {
                //���g�f��UI�M�ƥ�(������)
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
}
