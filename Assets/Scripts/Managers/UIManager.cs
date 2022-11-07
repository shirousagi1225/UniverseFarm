using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainCanvas;

    private bool canOpenSecUI = true;
    private bool isInfoBarOpen = false;
    private bool isSecUIOpen = false;

    public bool isMainUIOpen
    {
        set
        {
            canOpenSecUI = !value;
        }
    }

    //可改善：透過事件讓個別UI可以根據自己需求使用方法
    public void ShowInfoBar()
    {
        //測試用,正式true改為!isInfoBarOpen
        isInfoBarOpen = true;
        if (isInfoBarOpen)
        {
            mainCanvas.transform.GetChild(1).gameObject.SetActive(isInfoBarOpen);
        }
        else
        {
            mainCanvas.transform.GetChild(1).gameObject.SetActive(isInfoBarOpen);
        }
    }

    //須加入參數用於判斷該開啟哪個UI
    //須限制只開啟種子類型的物品欄
    public void ShowSecUI()
    {
        if (canOpenSecUI)
        {
            isSecUIOpen = !isSecUIOpen;
            if (isSecUIOpen)
            {
                for (int i = 0; i < mainCanvas.transform.childCount; i++)
                {
                    mainCanvas.transform.GetChild(i).gameObject.SetActive(false);
                }
                mainCanvas.transform.GetChild(9).gameObject.SetActive(isSecUIOpen);
            }
            else
            {
                for (int i = 0; i < mainCanvas.transform.childCount; i++)
                {
                    mainCanvas.transform.GetChild(i).gameObject.SetActive(true);
                }
                mainCanvas.transform.GetChild(9).gameObject.SetActive(isSecUIOpen);
            }
        }
    }

    //須寫啟動購買UI方法
}
