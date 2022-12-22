using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    //�O�s�ƾڡGInventoryManager
    public Dictionary<string, int> coinCountDict = new();
    public List<ItemName> itemList;
    public Dictionary<string,int> itemCountDict = new();
    public List<ItemName> seedList;
    public Dictionary<string, int> seedCountDict=new();

    //�O�s�ƾڡGFarmlandManager
    public List<FarmlandName> farmlandList;

    //�O�s�ƾڡGObjectManager
    public Dictionary<string, bool> cropPokedexAvailableDict;
}
