using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.UpdateMoneyUIEvent += OnUpdateMoneyUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateMoneyUIEvent -= OnUpdateMoneyUIEvent;
    }

    private void OnUpdateMoneyUIEvent(ItemDetails itemDetails)
    {
        //���g����������ƶq�覡
    }
}