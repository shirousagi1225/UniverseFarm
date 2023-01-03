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

    //�ݫ�ҭn�ǤJ����Ѽ�(�D�n�\��G�}���lUI ��s��r ��ܸӮ��ӫ~�ƶq ��ܸӮ��ӫ~�Ϥ�)
    private void OnShowUniversalUIEvent(UniversalUIDetails UITypeDetails,ItemDetails itemDetails,int count)
    {
        //���ھڼ��y�~�����ӧO�s�W
        Instantiate(itemBar, rewardContent.transform);
        rewardContent.transform.GetChild(rewardType).transform.GetChild(0).GetComponent<ItemBarSlotUI>().SetItem(itemDetails, count);
        rewardType ++;

        //�򥻸�T��l��
        if (title.text == "")
        {
            //���ھڳq��UI�����i��UI�]�m
            title.text = UITypeDetails.title;

            //�ҰʤlUI
            EventHandler.CallShowSecUIEvent("SecCanvas", true, false);
            panel.SetActive(true);
            //�ݧP�_�q��UI����,�����۲Ū�UI
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

    //�����q��UI��k
    public void CloseUniversalUI()
    {
        panel.SetActive(false);
        title.text = "";
        RemoveAllChildren(rewardContent);
        rewardType = 0;
        //�P�_�O�_���}�Ұӫ�UI
        if (GameObject.Find("ShopPanel") == null)
        {
            //�P�_�O�_���}�ҥDUI
            if (GameObject.Find("MainCanvas").GetComponent<CanvasGroup>().alpha == 0f)
                EventHandler.CallShowSecUIEvent("SecCanvas", false, true);
            else
                EventHandler.CallShowSecUIEvent("SecCanvas", false, false);
        }
    }

    //���g���������k

    //�M���Ҧ����y��k
    private void RemoveAllChildren(GameObject rewardContent)
    {
        for (int i = 0; i < rewardContent.transform.childCount; i++)
            Destroy(rewardContent.transform.GetChild(i).gameObject);
    }
}
