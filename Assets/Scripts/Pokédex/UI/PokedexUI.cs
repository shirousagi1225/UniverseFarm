using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokedexUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject cropItem;
    public GameObject cropInfo;

    //開啟圖鑑按鈕方法
    public void PokedexButton()
    {
        //啟動子UI
        EventHandler.CallShowSecUIEvent("TriCanvas", true, true);
        panel.SetActive(true);
    }

    //開啟種子圖鑑詳情方法
    public void CropButton(Sprite cropInfoSprite)
    {
        cropInfo.transform.GetChild(1).GetComponent<Image>().sprite = cropInfoSprite;
        cropItem.SetActive(false);
        cropInfo.SetActive(true);
    }

    //返回圖鑑總覽方法
    public void BackButton()
    {
        cropInfo.SetActive(false);
        cropItem.SetActive(true);
    }

    //關閉圖鑑按鈕方法
    public void CloseButton()
    {
        panel.SetActive(false);
        EventHandler.CallShowSecUIEvent("TriCanvas", false, true);
    }
}
