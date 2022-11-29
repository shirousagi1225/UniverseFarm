using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainCanvas;
    public GameObject SecCanvas;
    public GameObject hintButton;

    //用於背包欄UI
    private bool canOpenBackpackBarUI = true;
    private bool isBackpackBarUIOpen = false;

    //用於背包欄UI
    public bool isMainUIOpen
    {
        set{ canOpenBackpackBarUI = !value; }
    }

    private void OnEnable()
    {
        EventHandler.ShowSecUIEvent += OnShowSecUIEvent;
        EventHandler.UpdateHintUIEvent += OnUpdateHintUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.ShowSecUIEvent -= OnShowSecUIEvent;
        EventHandler.UpdateHintUIEvent -= OnUpdateHintUIEvent;
    }

    //需思考要傳入什麼參數(主要功能：開關子UI 更新文字 顯示該消耗品數量 顯示該消耗品圖片)
    private void OnShowSecUIEvent(bool canOpenSecUI)
    {
        if (canOpenSecUI)
        {
            ShowSecUI(canOpenSecUI);
        }
        else
        {
            CloseSecUI(canOpenSecUI);
        }
    }

    //暫定(可修改)：更新提示UI事件,統計各狀態提示出現數量(程式碼多的話考慮單獨寫一個腳本)
    private void OnUpdateHintUIEvent(Farmland farmland, Sprite farmlandStateSprite)
    {
        //需參照InventoryUI腳本,動態更新提示UI內各狀態提示數量及順序
        if (!hintButton.activeInHierarchy)
            hintButton.SetActive(true);

        hintButton.GetComponent<Image>().sprite = farmlandStateSprite;
    }

    //可改善：透過事件讓個別UI可以根據自己需求使用方法
    //須加入參數用於判斷該開啟哪個UI
    //需判斷開啟種植UI是點擊農地,關閉則是點擊場景(只有單點場景才會關閉,有進行相機移動則不會)
    public void ShowBackpackBarUI()
    {
        if (canOpenBackpackBarUI)
        {
            isBackpackBarUIOpen = !isBackpackBarUIOpen;
            if (isBackpackBarUIOpen)
            {
                ShowSecUI(isBackpackBarUIOpen);
                SecCanvas.transform.GetChild(0).gameObject.SetActive(isBackpackBarUIOpen);

                //限制只開啟種子類型的物品欄
                SecCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(!isBackpackBarUIOpen);
                SecCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);
            }
            else
            {
                CloseSecUI(isBackpackBarUIOpen);
                SecCanvas.transform.GetChild(0).gameObject.SetActive(isBackpackBarUIOpen);

                SecCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(!isBackpackBarUIOpen);
                SecCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);
            }
        }
    }

    //須寫啟動購買UI方法

    private void ShowSecUI(bool toggleFactor)
    {
        mainCanvas.GetComponent<CanvasGroup>().alpha = 0f;
        mainCanvas.GetComponent<CanvasGroup>().interactable = !toggleFactor;
        mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = !toggleFactor;
        SecCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        SecCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
        SecCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
    }

    private void CloseSecUI(bool toggleFactor)
    {
        mainCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        mainCanvas.GetComponent<CanvasGroup>().interactable = !toggleFactor;
        mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = !toggleFactor;
        SecCanvas.GetComponent<CanvasGroup>().alpha = 0f;
        SecCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
        SecCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
    }
}
