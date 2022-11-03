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
            //須寫判斷使用哪種UI來限制資訊初始化
            currentItem = itemName;
            currentSeed = itemDetails;
            beginDragParent = dragParent;

            itemImage.sprite = itemDetails.itemSprite;
            itemImage.SetNativeSize();
        }
        //須寫抓取持有物數量方式
    }

    public void SetEmpty()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //須寫判斷使用哪種UI來限制拖動功能
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
