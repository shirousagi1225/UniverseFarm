using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UniversalUI : MonoBehaviour
{
    public GameObject panel;
    public Text title;
    public GameObject rewardContent;
    public GameObject confirmType;
    public GameObject receiveType;
    public GameObject itemBar;

    private int rewardType;

    private void OnEnable()
    {
        EventHandler.ShowUniversalUIEvent += OnShowUniversalUIEvent;
        rewardType = 0;
    }

    private void OnDisable()
    {
        EventHandler.ShowUniversalUIEvent -= OnShowUniversalUIEvent;
    }

    //需思考要傳入什麼參數(主要功能：開關子UI 更新文字 顯示該消耗品數量 顯示該消耗品圖片)
    private void OnShowUniversalUIEvent(UniversalUIDetails UITypeDetails,ItemDetails itemDetails,int count)
    {
        //須根據獎勵品種類個別新增
        Instantiate(itemBar, rewardContent.transform);
        rewardContent.transform.GetChild(rewardType).transform.GetChild(0).GetComponent<ItemBarSlotUI>().SetItem(itemDetails, count);
        rewardType ++;

        //基本資訊初始化
        if (title.text == "")
        {
            //須根據通用UI種類進行UI設置
            title.text = UITypeDetails.title;

            //啟動子UI
            EventHandler.CallShowSecUIEvent("SecCanvas", true, false);
            panel.SetActive(true);
            //需判斷通用UI種類,切換相符的UI
            if (UITypeDetails.UIType == UniversalUIType.CustomerSell)
            {
                confirmType.SetActive(false);
                receiveType.SetActive(true);
            }
            else
            {
                confirmType.SetActive(true);
                receiveType.SetActive(false);
            }
        }
    }

    //關閉通用UI方法
    public void CloseUniversalUI()
    {
        panel.SetActive(false);
        title.text = "";
        RemoveAllChildren(rewardContent);
        rewardType = 0;
        //判斷是否有開啟商城UI
        if (GameObject.Find("ShopPanel") == null)
        {
            //判斷是否有開啟主UI
            if (GameObject.Find("MainCanvas").GetComponent<CanvasGroup>().alpha == 0f)
                EventHandler.CallShowSecUIEvent("SecCanvas", false, true);
            else
                EventHandler.CallShowSecUIEvent("SecCanvas", false, false);
        }
    }

    //須寫雙倍領取方法

    //清除所有獎勵方法
    private void RemoveAllChildren(GameObject rewardContent)
    {
        for (int i = 0; i < rewardContent.transform.childCount; i++)
            Destroy(rewardContent.transform.GetChild(i).gameObject);
    }
}
