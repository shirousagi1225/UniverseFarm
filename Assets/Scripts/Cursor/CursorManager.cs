using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

    private bool canClick;

    private void Update()
    {
        canClick = ObjectAtMousePosition();
        CameraManager.Instance.IsClick(canClick);

        if (canClick&&Input.GetMouseButtonDown(0))
        {
            ClickAction(ObjectAtMousePosition().gameObject);
        }
        else
        {
            CameraManager.Instance.InputType();
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
                /*if (farmland.isPlant == false)
                    farmland?.PlantAction();
                else
                    farmland?.FarmlandClicked();*/
                break;
        }
    }

    private Collider2D ObjectAtMousePosition()
    {
        return Physics2D.OverlapPoint(mouseWorldPos);
    }
}
