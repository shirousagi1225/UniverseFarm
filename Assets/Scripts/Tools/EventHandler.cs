using System;
using UnityEngine;

public static class EventHandler
{
    public static event Action<ItemDetails, int,bool> UpdateUIEvent;

    public static void CallUpdateUIEvent(ItemDetails itemDetails,int index, bool isFirst)
    {
        UpdateUIEvent?.Invoke(itemDetails,index, isFirst);
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
}
