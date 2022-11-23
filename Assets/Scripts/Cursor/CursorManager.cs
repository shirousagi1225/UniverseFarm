using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CursorManager : MonoBehaviour
{
    private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

    private bool canClick;

    private void Update()
    {
        canClick = ObjectAtMousePosition();
        CameraManager.Instance.IsClick(canClick);
        //Debug.Log(ObjectAtMousePosition());

        //����G�P�_�O�_�I��UI�]���F�ϧO�O��ª��I���ù��٬O�n�MUI����)
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
            FarmlandManager.Instance.CreateCrop(seedDetails, itemName, crop, dropObject);
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
                //���g�P�_�b�}�Һش�UI�ɨ�l����t�Υ\��Ҥ���ϥ�
                var farmland=clickObject.GetComponent<Farmland>();
                if (farmland.canPlant)
                    farmland?.PlantAction();
                else
                    farmland?.FarmlandClicked();
                //Debug.Log(farmland.canPlant);
                break;
            case "Crop":
                //���ն��q�G������k�g�bCrop.cs
                //�������q�G������k�g�b�U�ȵ{��
                var crop = clickObject.GetComponent<Crop>();
                crop?.CropClicked();
                break;
            case "Seed":
                var seed = clickObject.GetComponent<Seed>();
                seed?.SeedClicked();
                break;
        }
    }

    private Collider2D ObjectAtMousePosition()
    {
        //�W�[Layer�ѼƸѨM����˴��I���ؤz�Z�I�������D
        var isCustomer = Physics2D.OverlapPoint(mouseWorldPos, 1 << LayerMask.NameToLayer("Customer"));
        var isClickDetect = Physics2D.OverlapPoint(mouseWorldPos, 1 << LayerMask.NameToLayer("ClickDetect"));

        if (isCustomer)
            return isCustomer;
        else
            return isClickDetect;
    }
}
