using System;
using UnityEngine;
using UnityEngine.UI;

public static class EventHandler
{
    public static event Action<ItemDetails,ItemName, int,bool,string> UpdateInventoryUIEvent;
    public static void CallUpdateInventoryUIEvent(ItemDetails itemDetails, ItemName itemName, int index, bool isFirst,string inventoryType)
    {
        UpdateInventoryUIEvent?.Invoke(itemDetails, itemName, index, isFirst, inventoryType);
    }

    public static event Action<ItemDetails> UpdateMoneyUIEvent;
    public static void CallUpdateMoneyUIEvent(ItemDetails itemDetails)
    {
        UpdateMoneyUIEvent?.Invoke(itemDetails);
    }

    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    public static event Action AfterSceneLoadedEvent;
    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }

    public static event Action<ItemDetails,ItemName,GameObject> ItemDragEvent;
    public static void CallItemDragEvent(ItemDetails seedDetails, ItemName itemName,GameObject crop)
    {
        ItemDragEvent?.Invoke(seedDetails, itemName, crop);
    }

    public static event Action<FarmlandName, CropStateDetails> SetGrowTimeEvent;
    public static void CallSetGrowTimeEvent(FarmlandName farmlandName, CropStateDetails cropStateDetails)
    {
        SetGrowTimeEvent?.Invoke(farmlandName,cropStateDetails);
    }

    public static event Action<FarmlandName, bool, Text> UpdateGrowTimeEvent;
    public static void CallUpdateGrowTimeEvent(FarmlandName farmlandName, bool isInfoBarOpen, Text growTime)
    {
        UpdateGrowTimeEvent?.Invoke(farmlandName, isInfoBarOpen, growTime);
    }

    public static event Action<Farmland> UpdateFarmlandStateEvent;
    public static void CallUpdateFarmlandStateEvent(Farmland farmland)
    {
        UpdateFarmlandStateEvent?.Invoke(farmland);
    }

    public static event Action<Farmland, Sprite> UpdateHintUIEvent;
    public static void CallUpdateHintUIEvent(Farmland farmland, Sprite farmlandStateSprite)
    {
        UpdateHintUIEvent?.Invoke(farmland, farmlandStateSprite);
    }
}
