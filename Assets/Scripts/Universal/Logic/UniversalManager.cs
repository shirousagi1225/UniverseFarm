using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalManager : Singleton<UniversalManager>
{
    public UniversalUIList_SO UITypeData;

    private void OnEnable()
    {
        EventHandler.SetUniversalUIEvent += OnSetUniversalUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.SetUniversalUIEvent -= OnSetUniversalUIEvent;
    }

    private void OnSetUniversalUIEvent(UniversalUIType UIType, ItemDetails itemDetails, int count)
    {
        EventHandler.CallShowUniversalUIEvent(UITypeData.GetUniversalUIDetails(UIType), itemDetails, count);
    }
}
