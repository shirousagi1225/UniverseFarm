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

    //seedName參數於測試使用,正式將寫在增加種子方法
    public void AddItem(ItemName itemName, ItemName seedName, int itemCount)
    {
        if (!itemList.Contains(itemName))
        {
            if (itemName.ToString()== "StarStone"|| itemName.ToString() == "Obsidian") 
            {
                //須寫貨幣UI和事件(未完成)
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

    //增加種子方法(未完成)
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

    //減少持有量方法(未完成)
    public void ReduceItem(ItemName itemName, int itemCount)
    {
        if (itemName.ToString() == "StarStone" || itemName.ToString() == "Obsidian")
        {
            //須啟動購買UI
            //須加入判斷:是否有足夠金額購買
            if (itemCount <= itemData.GetItemDetails(itemName).itemCount)
            {
                //須寫貨幣UI和事件(未完成)
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
