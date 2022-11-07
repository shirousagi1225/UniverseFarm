using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemContent;
    public GameObject seedContent;
    public GameObject itemUnit;
    //public int currentIndex;

    private SlotUI slotUI;
    //private Dictionary<int, int> itemSlotDict = new Dictionary<int, int>();
    //private Dictionary<int, int> seedSlotDict = new Dictionary<int, int>();
    //private int slotIndex;

    private void OnEnable()
    {
        //slotIndex = 0;
        EventHandler.UpdateInventoryUIEvent += OnUpdateInventoryUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateInventoryUIEvent -= OnUpdateInventoryUIEvent;
    }

    private void OnUpdateInventoryUIEvent(ItemDetails itemDetails, ItemName itemName, int index,bool isFirst, string inventoryType)
    {
        if (itemDetails==null)
        {
            Debug.Log("null");
            //刪除背包物品
            if (inventoryType == "Item")
                slotUI = itemContent.transform.GetChild(index).transform.GetChild(1).GetComponent<SlotUI>();
            else if (inventoryType == "Seed")
                slotUI = seedContent.transform.GetChild(index).transform.GetChild(1).GetComponent<SlotUI>();
            slotUI.SetEmpty();
            //currentIndex = -1;
        }
        else if(isFirst)
        {
            Instantiate(itemUnit, itemContent.transform);
            //slotDict.Add(index, slotIndex);
            if (inventoryType== "Item")
                slotUI = itemContent.transform.GetChild(index).transform.GetChild(1).GetComponent<SlotUI>();
            else if(inventoryType == "Seed")
                slotUI = seedContent.transform.GetChild(index).transform.GetChild(1).GetComponent<SlotUI>();
            //currentIndex =index;
            slotUI.SetItem(itemDetails, itemName, isFirst, inventoryType, UIManager.Instance.mainCanvas.transform);
            //slotIndex++;
        }
        else
        {
            if (inventoryType == "Item")
                slotUI = itemContent.transform.GetChild(index).transform.GetChild(1).GetComponent<SlotUI>();
            else if (inventoryType == "Seed")
                slotUI = seedContent.transform.GetChild(index).transform.GetChild(1).GetComponent<SlotUI>();
            slotUI.SetItem(itemDetails, itemName, isFirst, inventoryType, UIManager.Instance.mainCanvas.transform);
        }
    }
}
