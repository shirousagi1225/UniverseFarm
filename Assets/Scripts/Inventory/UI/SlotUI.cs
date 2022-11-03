using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image itemImage;
    public GameObject crop;

    private ItemName currentItem;
    private ItemDetails currentSeed;
    private Vector3 startPos;
    private Transform beginDragParent;
    private Transform startParent;

    public void SetItem(ItemDetails itemDetails, ItemName itemName, bool isFirst,Transform dragParent)
    {
        if (isFirst)
        {
            //���g�P�_�ϥέ���UI�ӭ����T��l��
            currentItem = itemName;
            currentSeed = itemDetails;
            beginDragParent = dragParent;

            itemImage.sprite = itemDetails.itemSprite;
            itemImage.SetNativeSize();
        }
        //���g����������ƶq�覡
    }

    public void SetEmpty()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //���g�P�_�ϥέ���UI�ӭ����ʥ\��
        startPos = transform.position;
        startParent = transform.parent;
        transform.SetParent(beginDragParent.parent); 
    }

    public void OnDrag(PointerEventData eventData)
    {
        #if !UNITY_EDITOR && UNITY_ANDROID
            transform.position = Input.GetTouch(0).position;
        #else
            transform.position = Input.mousePosition;
        #endif
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(startParent);
        transform.position = startPos;
        EventHandler.CallItemDragEvent(currentSeed, currentItem, crop);
    }
}
