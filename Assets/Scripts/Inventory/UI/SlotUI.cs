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

    public void SetItem(ItemDetails itemDetails, ItemName itemName, bool isFirst, string inventoryType, Transform dragParent)
    {
        if (isFirst)
        {
            //須寫判斷初始化哪種持有物類型的資訊(用inventoryType)
            currentItem = itemName;
            currentSeed = itemDetails;
            beginDragParent = dragParent;

            itemImage.sprite = itemDetails.itemSprite;
            itemImage.SetNativeSize();
        }
        //抓取持有物數量
        if (itemDetails.itemCount > 99)
            transform.parent.gameObject.transform.GetChild(2).GetComponent<Text>().text = "99+";
        else
            transform.parent.gameObject.transform.GetChild(2).GetComponent<Text>().text = itemDetails.itemCount.ToString();
    }

    public void SetEmpty()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //判斷使用哪種UI來限制拖動功能
        if (GameObject.Find("BackpackButton")==null)
        {
            startPos = transform.position;
            startParent = transform.parent;
            transform.SetParent(beginDragParent);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameObject.Find("BackpackButton") == null)
        {
            #if !UNITY_EDITOR && UNITY_ANDROID
                transform.position = Input.GetTouch(0).position;
            #else
                transform.position = Input.mousePosition;
            #endif
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameObject.Find("BackpackButton") == null)
        {
            transform.SetParent(startParent);
            transform.SetSiblingIndex(1);
            transform.position = startPos;
            EventHandler.CallItemDragEvent(currentSeed, currentItem, crop);
        }
    }
}
