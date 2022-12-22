using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokedexUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject cropItem;
    public GameObject cropInfo;

    //�}�ҹ�Ų���s��k
    public void PokedexButton()
    {
        //�ҰʤlUI
        EventHandler.CallShowSecUIEvent("TriCanvas", true, true);
        panel.SetActive(true);
    }

    //�}�Һؤl��Ų�Ա���k
    public void CropButton(Sprite cropInfoSprite)
    {
        cropInfo.transform.GetChild(1).GetComponent<Image>().sprite = cropInfoSprite;
        cropItem.SetActive(false);
        cropInfo.SetActive(true);
    }

    //��^��Ų�`����k
    public void BackButton()
    {
        cropInfo.SetActive(false);
        cropItem.SetActive(true);
    }

    //������Ų���s��k
    public void CloseButton()
    {
        panel.SetActive(false);
        EventHandler.CallShowSecUIEvent("TriCanvas", false, true);
    }
}
