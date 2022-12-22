using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    //保存數據：InventoryManager
    public Dictionary<string, int> coinCountDict = new();
    public List<ItemName> itemList;
    public Dictionary<string,int> itemCountDict = new();
    public List<ItemName> seedList;
    public Dictionary<string, int> seedCountDict=new();

    //保存數據：FarmlandManager
    public List<FarmlandName> farmlandList;

    //保存數據：ObjectManager
    public Dictionary<string, bool> cropPokedexAvailableDict;
}
