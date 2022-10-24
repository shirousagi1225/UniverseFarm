using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject content;
    public GameObject itemUnit;
    public int currentIndex;

    private SlotUI slotUI;
    private Dictionary<int, int> slotDict = new Dictionary<int, int>();
    private int slotIndex;

    private void OnEnable()
    {
        slotIndex = 0;
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
    }

    private void OnUpdateUIEvent(ItemDetails itemDetails, int index,bool isFirst)
    {
        if (itemDetails==null)
        {
            //等減少持有量方法寫完須增加刪除背包物品(未完成)
            slotUI.SetEmpty();
            currentIndex = -1;
        }
        else if(isFirst)
        {
            Instantiate(itemUnit, content.transform);
            slotDict.Add(index, slotIndex);
            slotUI = content.transform.GetChild(slotDict[index]).transform.GetChild(1).GetComponent<SlotUI>();
            currentIndex =index;
            slotUI.SetItem(itemDetails);
            slotIndex++;
        }
        else
        {
            slotUI = content.transform.GetChild(slotDict[index]).transform.GetChild(1).GetComponent<SlotUI>();
            currentIndex = index;
            slotUI.SetItem(itemDetails);
        }
    }
}
