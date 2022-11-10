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

    private bool isInfoBarOpen = false;

    public void CropClicked()
    {
        //���եζ��H�����ɶ��P�_�O�_�i����,�����אּ�U�ȬO�_�ʶR�P�_
        ShowInfoBar();
        Harvest();
    }

    public void SetCrop(ItemDetails seedDetails, ItemName itemName)
    {
        //���ե�,����seedName��cropName���ȹ��
        seedName = itemName;
        cropName = seedDetails.itemName;
        //���ե�,�������ݭn�[
        GetComponent<SpriteRenderer>().sprite = seedDetails.itemSprite;
    }

    //�@��������k(������)
    private void Harvest()
    {
        InventoryManager.Instance.AddItem(cropName, seedName, FarmlandManager.Instance.Produce(seedName));
        transform.parent.GetComponent<Collider2D>().enabled = true;
        Destroy(gameObject);
    }

    //���A��T����ܤ�k(������)
    public void ShowInfoBar()
    {
        //���P�_�I����ӧ@��,�����W�u������I��@������T��,�}�ҤU�ӫe���������e�@��
        //���ե�,����true�אּ!isInfoBarOpen
        isInfoBarOpen = true;
        if (isInfoBarOpen)
            transform.parent.GetChild(1).GetComponent<CanvasGroup>().alpha=1.0f;
        else
            transform.parent.GetChild(1).GetComponent<CanvasGroup>().alpha = 0f;
        transform.parent.GetChild(1).GetComponent<CanvasGroup>().interactable = isInfoBarOpen;
        transform.parent.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = isInfoBarOpen;
        EventHandler.CallUpdateGrowTimeEvent(transform.parent.GetComponent<Farmland>().farmlandName, isInfoBarOpen, 
            transform.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>());
    }
}
