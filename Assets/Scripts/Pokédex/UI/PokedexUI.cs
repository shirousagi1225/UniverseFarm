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

    //�}�ҹ�Ų���s��k
    public void PokedexButton()
    {
        //�ҰʤlUI
        EventHandler.CallShowSecUIEvent("TriCanvas", true, true);
        panel.SetActive(true);
    }

    //�����ؤl��Ų��k
    public void ChangeCrop()
    {
        cropItem.SetActive(false);
        roleItem.SetActive(true);
    }

    //�����U�ȹ�Ų��k
    public void ChangeCustomer()
    {
        roleItem.SetActive(false);
        cropItem.SetActive(true);
    }

    //�}�Һؤl��Ų�Ա���k
    public void CropButton(Sprite cropInfoSprite)
    {
        cropInfo.transform.GetChild(1).GetComponent<Image>().sprite = cropInfoSprite;
        cropItem.SetActive(false);
        cropInfo.SetActive(true);
    }

    //��^�ؤl��Ų�`����k
    public void BackCrop()
    {
        cropInfo.SetActive(false);
        cropItem.SetActive(true);
    }

    //�}���U�ȹ�Ų�Ա���k
    public void CustomerButton(Sprite ccustomerInfoSprite)
    {
        roleInfo.transform.GetChild(1).GetComponent<Image>().sprite = ccustomerInfoSprite;
        roleItem.SetActive(false);
        roleInfo.SetActive(true);
    }

    //��^�U�ȹ�Ų�`����k
    public void BackCustomer()
    {
        roleInfo.SetActive(false);
        roleItem.SetActive(true);
    }

    //�}���U�ȬG�Ƥ�k
    public void StoryButton()
    {
        roleInfo.SetActive(false);
        roleStory.SetActive(true);
    }

    //��^�U�ȹ�Ų�Ա���k
    public void BackCustomerInfo()
    {
        roleStory.SetActive(false);
        roleInfo.SetActive(true);
    }

    //������Ų���s��k
    public void CloseButton()
    {
        panel.SetActive(false);
        EventHandler.CallShowSecUIEvent("TriCanvas", false, true);
    }
}
