using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crop : MonoBehaviour
{
    public ItemName seedName;
    public ItemName cropName;

    //���ե�,����3�אּ0
    private int _growthStage=3;
    private bool isInfoBarOpen = false;

    public int growthStage
    {
        get
        {
            return _growthStage;
        }

        set
        {
            _growthStage=value;
        }
    }

    public void CropClicked()
    {
        //���եζ��H�������q�P�_�O�_�i����,�����אּ�U�ȬO�_�ʶR�P�_(�w�ק�)
        StartCoroutine(ShowInfoBar());
    }

    public void SetCrop(ItemDetails seedDetails, ItemName itemName)
    {
        //���ե�,����seedName��cropName���ȹ��(�w�ק�)
        seedName = seedDetails.itemName;
        cropName = itemName;
        //���ե�,�������ݭn�[
        _growthStage = 0;
    }

    //�@��������k(������)
    public IEnumerator Harvest()
    {
        if (isInfoBarOpen)
            yield return ShowInfoBar();
        else
            yield return new WaitForSeconds(0);

        EventHandler.CallUpdateFarmlandStateEvent(transform.parent.GetComponent<Farmland>());
        transform.parent.GetComponent<Collider2D>().enabled = true;
        Destroy(gameObject);
    }

    //���A��T����ܤ�k(������)
    private IEnumerator ShowInfoBar()
    {
        //���P�_�I����ӧ@��,�����W�u������I��@������T��,�}�ҤU�ӫe���������e�@��
        isInfoBarOpen = !isInfoBarOpen;

        EventHandler.CallUpdateGrowTimeEvent(transform.parent.GetComponent<Farmland>().farmlandName, isInfoBarOpen,
            transform.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>());

        yield return new WaitForSeconds(1);

        if (isInfoBarOpen)
            transform.parent.GetChild(1).GetComponent<CanvasGroup>().alpha=1.0f;
        else
            transform.parent.GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
        transform.parent.GetChild(1).GetComponent<CanvasGroup>().interactable = isInfoBarOpen;
        transform.parent.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = isInfoBarOpen;
    }
}
