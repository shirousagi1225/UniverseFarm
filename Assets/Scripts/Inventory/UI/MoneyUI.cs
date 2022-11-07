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
        //抓取持有物數量
        if(transform.GetChild(0).name==itemDetails.itemName.ToString())
            transform.GetChild(1).GetComponent<Text>().text= itemDetails.itemCount.ToString();
    }
}
