using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
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

    private void ClickAction(GameObject clickObject)
    {
        switch (clickObject.tag)
        {
            case "Teleport":
                var teleport = clickObject.GetComponent<Teleport>();
                teleport?.TeleportToScene();
                break;
            /*case "Farmland":
                var farmland=clickObject.GetComponent<Farmland>();
                if (farmland.isPlant == false)
                    farmland?.PlantAction();
                else
                    farmland?.FarmlandClicked();
                break;*/
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
