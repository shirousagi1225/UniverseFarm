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

    //�ݫ�ҭn�ǤJ����Ѽ�(�D�n�\��G�}���lUI ��s��r ��ܸӮ��ӫ~�ƶq ��ܸӮ��ӫ~�Ϥ�)
    private void OnShowUniversalUIEvent(UniversalUIDetails UITypeDetails,ItemDetails itemDetails,int count)
    {
        //���ھڳq��UI�����i��UI�]�m
        title.text = UITypeDetails.title;
        receiveOK.text = UITypeDetails.buttonText01;
        doubleReceiveOK.text = UITypeDetails.buttonText02;
        //���ھڼ��y�~�����ӧO�s�W
        Instantiate(itemBar, rewardContent.transform);
        rewardContent.transform.GetChild(0).transform.GetChild(0).GetComponent<ItemBarSlotUI>().SetItem(itemDetails, count);

        //�ҰʤlUI
        EventHandler.CallShowSecUIEvent(true,false);
        panel.SetActive(true);
    }

    //�����q��UI��k
    public void CloseUniversalUI()
    {
        panel.SetActive(false);
        title.text = "";
        receiveOK.text = "";
        doubleReceiveOK.text = "";
        RemoveAllChildren(rewardContent);
        //�ݧP�_�O�_���}�ҥDUI
        if(GameObject.Find("MainCanvas").GetComponent<CanvasGroup>().alpha == 0f)
            EventHandler.CallShowSecUIEvent(false, true);
        else
            EventHandler.CallShowSecUIEvent(false,false);
    }

    //���g���������k

    //�M���Ҧ����y��k
    private void RemoveAllChildren(GameObject rewardContent)
    {
        for (int i = 0; i < rewardContent.transform.childCount; i++)
            Destroy(rewardContent.transform.GetChild(i).gameObject);
    }
}
