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
    public Text receiveOK;
    public Text doubleReceiveOK;
    public GameObject itemBar;

    private void OnEnable()
    {
        EventHandler.ShowUniversalUIEvent += OnShowUniversalUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.ShowUniversalUIEvent -= OnShowUniversalUIEvent;
    }

    //需思考要傳入什麼參數(主要功能：開關子UI 更新文字 顯示該消耗品數量 顯示該消耗品圖片)
    private void OnShowUniversalUIEvent(UniversalUIDetails UITypeDetails,ItemDetails itemDetails,int count)
    {
        //須根據通用UI種類進行UI設置
        title.text = UITypeDetails.title;
        receiveOK.text = UITypeDetails.buttonText01;
        doubleReceiveOK.text = UITypeDetails.buttonText02;
        //須根據獎勵品種類個別新增
        Instantiate(itemBar, rewardContent.transform);
        rewardContent.transform.GetChild(0).transform.GetChild(0).GetComponent<ItemBarSlotUI>().SetItem(itemDetails, count);

        //啟動子UI
        EventHandler.CallShowSecUIEvent(true,false);
        panel.SetActive(true);
    }

    //關閉通用UI方法
    public void CloseUniversalUI()
    {
        panel.SetActive(false);
        title.text = "";
        receiveOK.text = "";
        doubleReceiveOK.text = "";
        RemoveAllChildren(rewardContent);
        //需判斷是否有開啟主UI
        if(GameObject.Find("MainCanvas").GetComponent<CanvasGroup>().alpha == 0f)
            EventHandler.CallShowSecUIEvent(false, true);
        else
            EventHandler.CallShowSecUIEvent(false,false);
    }

    //須寫雙倍領取方法

    //清除所有獎勵方法
    private void RemoveAllChildren(GameObject rewardContent)
    {
        for (int i = 0; i < rewardContent.transform.childCount; i++)
            Destroy(rewardContent.transform.GetChild(i).gameObject);
    }
}
