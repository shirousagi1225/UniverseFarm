using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainCanvas;
    public GameObject secCanvas;
    public GameObject triCanvas;
    public GameObject hintButton;

    //用於背包欄UI
    private bool canOpenBackpackBarUI = true;
    private bool isBackpackBarUIOpen = false;

    //判斷是由哪種方式開啟背包欄UI(用於背包欄UI：分為背包按鈕及可種植農地)
    public bool isMainUIOpen
    {
        get { return !canOpenBackpackBarUI; }
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

    //切換主UI與次UI事件
    private void OnShowSecUIEvent(string canvas,bool canOpenSecUI, bool canSwitch)
    {
        if (canOpenSecUI)
        {
            ShowSecUI(canvas,canOpenSecUI, canSwitch);
        }
        else
        {
            CloseSecUI(canvas,canOpenSecUI, canSwitch);
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

    //顯示背包欄UI方法(未完成)
    public void ShowBackpackBarUI()
    {
        if (canOpenBackpackBarUI)
        {
            isBackpackBarUIOpen = !isBackpackBarUIOpen;
            if (isBackpackBarUIOpen)
            {
                ShowSecUI("SecCanvas",isBackpackBarUIOpen,true);
                secCanvas.transform.GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);

                //限制只開啟種子類型的物品欄
                secCanvas.transform.GetChild(1).GetChild(0).gameObject.SetActive(!isBackpackBarUIOpen);
                secCanvas.transform.GetChild(1).GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);
            }
            else
            {
                CloseSecUI("SecCanvas", isBackpackBarUIOpen,true);
                secCanvas.transform.GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);

                secCanvas.transform.GetChild(1).GetChild(0).gameObject.SetActive(!isBackpackBarUIOpen);
                secCanvas.transform.GetChild(1).GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);
            }
        }
    }

    //須寫啟動購買UI方法

    private void ShowSecUI(string canvas,bool toggleFactor, bool canSwitch)
    {
        //判斷是要切換主次UI或單獨開啟次UI
        if (canSwitch)
        {
            mainCanvas.GetComponent<CanvasGroup>().alpha = 0f;
            mainCanvas.GetComponent<CanvasGroup>().interactable = !toggleFactor;
            mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = !toggleFactor;
        }

        //判斷是要使用哪種次UI
        switch (canvas)
        {
            case "SecCanvas":
                secCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
                secCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
                secCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
                break;
            case "TriCanvas":
                triCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
                triCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
                triCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
                break;
        }
    }

    private void CloseSecUI(string canvas,bool toggleFactor, bool canSwitch)
    {
        //判斷是要切換主次UI或單獨開啟次UI
        if (canSwitch)
        {
            mainCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
            mainCanvas.GetComponent<CanvasGroup>().interactable = !toggleFactor;
            mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = !toggleFactor;
        }

        //判斷是要使用哪種次UI
        switch (canvas)
        {
            case "SecCanvas":
                secCanvas.GetComponent<CanvasGroup>().alpha = 0f;
                secCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
                secCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
                break;
            case "TriCanvas":
                triCanvas.GetComponent<CanvasGroup>().alpha = 0f;
                triCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
                triCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
                break;
        }
    }
}
