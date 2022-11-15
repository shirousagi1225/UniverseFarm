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

    private void Start()
    {
        EventHandler.CallUpdateMoneyUIEvent(itemData.GetItemDetails(ItemName.StarStone));
        EventHandler.CallUpdateMoneyUIEvent(itemData.GetItemDetails(ItemName.Obsidian));
    }

    //seedName參數於測試使用,正式將寫在增加種子方法(已修改)
    public void AddItem(ItemName itemName, int itemCount)
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
                itemData.GetItemDetails(itemName).itemCount += itemCount;
                EventHandler.CallUpdateInventoryUIEvent(itemData.GetItemDetails(itemName), ItemName.None, itemList.Count-1,true,"Item");
            }
        }
        else
        {
            itemData.GetItemDetails(itemName).itemCount += itemCount;
            EventHandler.CallUpdateInventoryUIEvent(itemData.GetItemDetails(itemName), ItemName.None, itemList.FindIndex(i => i == itemName), false, "Item");
        }
    }

    //增加種子方法(未完成)
    public void AddSeed(ItemName seedName, ItemName itemName, int seedCount)
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
    }

    //減少持有量方法(未完成)
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
                //不足數量告知無法購買
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
                //不足數量告知無法出售
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
