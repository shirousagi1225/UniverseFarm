using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokedexUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject cropItem;
    public GameObject cropInfo;
    public GameObject roleItem;
    public GameObject roleInfo;
    public GameObject roleStory;

    //開啟圖鑑按鈕方法
    public void PokedexButton()
    {
        //啟動子UI
        EventHandler.CallShowSecUIEvent("TriCanvas", true, true);
        panel.SetActive(true);
    }

    //切換種子圖鑑方法
    public void ChangeCrop()
    {
        cropItem.SetActive(false);
        roleItem.SetActive(true);
    }

    //切換顧客圖鑑方法
    public void ChangeCustomer()
    {
        roleItem.SetActive(false);
        cropItem.SetActive(true);
    }

    //開啟種子圖鑑詳情方法
    public void CropButton(Sprite cropInfoSprite)
    {
        cropInfo.transform.GetChild(1).GetComponent<Image>().sprite = cropInfoSprite;
        cropItem.SetActive(false);
        cropInfo.SetActive(true);
    }

    //返回種子圖鑑總覽方法
    public void BackCrop()
    {
        cropInfo.SetActive(false);
        cropItem.SetActive(true);
    }

    //開啟顧客圖鑑詳情方法
    public void CustomerButton(Sprite ccustomerInfoSprite)
    {
        roleInfo.transform.GetChild(1).GetComponent<Image>().sprite = ccustomerInfoSprite;
        roleItem.SetActive(false);
        roleInfo.SetActive(true);
    }

    //返回顧客圖鑑總覽方法
    public void BackCustomer()
    {
        roleInfo.SetActive(false);
        roleItem.SetActive(true);
    }

    //開啟顧客故事方法
    public void StoryButton()
    {
        roleInfo.SetActive(false);
        roleStory.SetActive(true);
    }

    //返回顧客圖鑑詳情方法
    public void BackCustomerInfo()
    {
        roleStory.SetActive(false);
        roleInfo.SetActive(true);
    }

    //關閉圖鑑按鈕方法
    public void CloseButton()
    {
        panel.SetActive(false);
        EventHandler.CallShowSecUIEvent("TriCanvas", false, true);
    }
}
