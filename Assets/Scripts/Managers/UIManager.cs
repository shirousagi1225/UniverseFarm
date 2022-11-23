using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainCanvas;
    public GameObject hintButton;

    private bool canOpenSecUI = true;
    private bool isSecUIOpen = false;

    public bool isMainUIOpen
    {
        set
        {
            canOpenSecUI = !value;
        }
    }

    private void OnEnable()
    {
        EventHandler.UpdateHintUIEvent += OnUpdateHintUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateHintUIEvent -= OnUpdateHintUIEvent;
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
    public void ShowSecUI()
    {
        if (canOpenSecUI)
        {
            isSecUIOpen = !isSecUIOpen;
            if (isSecUIOpen)
            {
                for (int i = 0; i < mainCanvas.transform.childCount; i++)
                {
                    if(mainCanvas.transform.GetChild(i).name!= "HintButton")
                        mainCanvas.transform.GetChild(i).gameObject.SetActive(false);
                }
                mainCanvas.transform.GetChild(9).gameObject.SetActive(isSecUIOpen);
                //限制只開啟種子類型的物品欄
                mainCanvas.transform.GetChild(9).GetChild(0).gameObject.SetActive(!isSecUIOpen);
                mainCanvas.transform.GetChild(9).GetChild(1).gameObject.SetActive(isSecUIOpen);
            }
            else
            {
                for (int i = 0; i < mainCanvas.transform.childCount; i++)
                {
                    if (mainCanvas.transform.GetChild(i).name != "HintButton")
                        mainCanvas.transform.GetChild(i).gameObject.SetActive(true);
                }
                mainCanvas.transform.GetChild(9).GetChild(0).gameObject.SetActive(!isSecUIOpen);
                mainCanvas.transform.GetChild(9).GetChild(1).gameObject.SetActive(isSecUIOpen);
                mainCanvas.transform.GetChild(9).gameObject.SetActive(isSecUIOpen);
                
            }
        }
    }

    //須寫啟動購買UI方法
}
