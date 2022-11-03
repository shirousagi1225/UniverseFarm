using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CursorManager : MonoBehaviour
{
    public GameObject mainCanvas;
    private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

    private bool canClick;

    private void Update()
    {
        canClick = ObjectAtMousePosition();
        CameraManager.Instance.IsClick(canClick);

        //桌機：判斷是否點擊UI（為了區別是單純的點擊螢幕還是要和UI互動)
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (canClick && Input.GetMouseButtonDown(0))
            {
                #if !UNITY_EDITOR && UNITY_ANDROID
                    bool isTouchUIElement = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

                    if(!isTouchUIElement)
                    {
                        ClickAction(ObjectAtMousePosition().gameObject);
                    }
                #else
                    ClickAction(ObjectAtMousePosition().gameObject);
                #endif
            }
            else
            {
                CameraManager.Instance.InputType();
            }
        }
    }

    private void OnEnable()
    {
        EventHandler.ItemDragEvent += OnItemDragEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemDragEvent -= OnItemDragEvent;
    }

    private void OnItemDragEvent(ItemDetails seedDetails, ItemName itemName,GameObject crop)
    {
        var dropObject = ObjectAtMousePosition();

        if (canClick && dropObject.gameObject.tag == "Farmland")
        {
            Debug.Log("yes");
            //增加UIManager後須將mainCanvas去除
            FarmlandManager.Instance.CreateCrop(seedDetails, itemName, crop, dropObject, mainCanvas);
        }
    }

    private void ClickAction(GameObject clickObject)
    {
        switch (clickObject.tag)
        {
            case "Teleport":
                var teleport = clickObject.GetComponent<Teleport>();
                teleport?.TeleportToScene();
                break;
            case "Farmland":
                var farmland=clickObject.GetComponent<Farmland>();
                if (farmland.canPlant)
                    farmland?.PlantAction(mainCanvas);
                else
                    farmland?.FarmlandClicked();
                //Debug.Log(farmland.canPlant);
                break;
            case "Crop":
                //測試階段：收成方法寫在Crop.cs
                var crop = clickObject.GetComponent<Crop>();
                crop?.CropClicked();
                //正式階段：收成方法寫在小圖示程式
                break;
        }
    }

    private Collider2D ObjectAtMousePosition()
    {
        return Physics2D.OverlapPoint(mouseWorldPos);
    }
}
