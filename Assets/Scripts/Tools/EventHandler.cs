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

    public static event Action<ClientDetails, int> SetClientProbabilityEvent;
    public static void CallSetClientProbabilityEvent(ClientDetails clientDetails, int favoriteState)
    {
        SetClientProbabilityEvent?.Invoke(clientDetails, favoriteState);
    }

    public static event Action<Transform,Collider2D[]> GetOtherCustomerEvent;
    public static void CallGetOtherCustomerEvent(Transform customer, Collider2D[] collider2Ds)
    {
        GetOtherCustomerEvent?.Invoke(customer, collider2Ds);
    }

    public static event Action<string,bool,bool> ShowSecUIEvent;
    public static void CallShowSecUIEvent(string canvas,bool canOpenSecUI,bool canSwitch)
    {
        ShowSecUIEvent?.Invoke(canvas,canOpenSecUI, canSwitch);
    }

    public static event Action<ClientDetails, string> ShowDialogueEvent;
    public static void CallShowDialogueEvent(ClientDetails clientDetails, string dialogue)
    {
        ShowDialogueEvent?.Invoke(clientDetails, dialogue);
    }

    public static event Action<ClientDetails> UpdateCustomerPokedexEvent;
    public static void CallUpdateCustomerPokedexEvent(ClientDetails clientDetails)
    {
        UpdateCustomerPokedexEvent?.Invoke(clientDetails);
    }

    public static event Action<ItemDetails> UpdateCropPokedexEvent;
    public static void CallUpdateCropPokedexEvent(ItemDetails itemDetails)
    {
        UpdateCropPokedexEvent?.Invoke(itemDetails);
    }

    public static event Action<UniversalUIType, ItemDetails,int> SetUniversalUIEvent;
    public static void CallSetUniversalUIEvent(UniversalUIType UIType, ItemDetails itemDetails,int count)
    {
        SetUniversalUIEvent?.Invoke(UIType, itemDetails, count);
    }

    public static event Action<UniversalUIDetails,ItemDetails,int> ShowUniversalUIEvent;
    public static void CallShowUniversalUIEvent(UniversalUIDetails UITypeDetails, ItemDetails itemDetails, int count)
    {
        ShowUniversalUIEvent?.Invoke(UITypeDetails,itemDetails, count);
    }

    public static event Action StartGameEvent;
    public static void CallStartGameEvent()
    {
        StartGameEvent?.Invoke();
    }
}
