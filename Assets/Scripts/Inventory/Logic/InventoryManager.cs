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

    public void AddItem(ItemName itemName,int itemCount)
    {
        if (!itemList.Contains(itemName))
        {
            if (itemName.ToString()== "StarStone"|| itemName.ToString() == "Obsidian") 
            {
                itemData.GetItemDetails(itemName).itemCount += itemCount;
            }
            else
            {
                itemList.Add(itemName);
                itemData.GetItemDetails(itemName).itemCount += itemCount;
                //���g�b�I�]�����Ұʤ���
                //EventHandler.CallUpdateUIEvent(itemData.GetItemDetails(itemName),itemList.Count-1);
            }
        }
        else
        {
            itemData.GetItemDetails(itemName).itemCount += itemCount;
        }
    }

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

    //���g��֫����q��k
}
