using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public Vector2 minPos;
    public Vector2 maxPos;

    private bool canClick;
    private bool canMoveCam=false;
    private bool isSingleFinger;
    private float scaleFactor = 0.01f;
    private float minDistance = 1f;
    private float maxDistance = 10f;
    private Vector2 lastSingleTouchPos;
    private Vector2 oldTouchPos1;
    private Vector2 oldTouchPos2;

    private void Start()
    {
        Input.multiTouchEnabled = true;
    }

    public void InputType()
    {
        #if !UNITY_EDITOR && UNITY_ANDROID
            MobileInput();
        #else
            DeskopInput();
        #endif
    }

    public void IsClick(bool isClick)
    {
        canClick = isClick;
    }

    private void DeskopInput()
    {
        if (canClick == false&&Input.GetMouseButtonDown(0))
        {
            lastSingleTouchPos = Input.mousePosition;
            canMoveCam = true;
        }
        else if (canMoveCam&&Input.GetMouseButton(0))
        {
            MoveCamera(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            canMoveCam = false;
        }
        else if (Input.GetAxis("Mouse ScrollWheel")!=0)
        {
            if (Camera.main.orthographicSize + (-Input.GetAxis("Mouse ScrollWheel")) > maxDistance)
                Camera.main.orthographicSize = maxDistance;
            else if (Camera.main.orthographicSize + (-Input.GetAxis("Mouse ScrollWheel")) < minDistance)
                Camera.main.orthographicSize = minDistance;
            else
                Camera.main.orthographicSize += -Input.GetAxis("Mouse ScrollWheel");
        }
    }

    private void MobileInput()
    {
        //須修復問題：轉換場景箭頭功能與畫面移動功能分離
        if (Input.touchCount <= 0)
        {
            isSingleFinger = true;
        }
        else if (isSingleFinger&&Input.touchCount == 1)
        {
            if (canClick == false && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                lastSingleTouchPos = Input.GetTouch(0).position;
                canMoveCam = true;
            }
            else if (canMoveCam&&Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                MoveCamera(Input.GetTouch(0).position);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                canMoveCam = false;
            }
        }
        else if (Input.touchCount == 2)
        {
            if (isSingleFinger)
            {
                oldTouchPos1 = Input.GetTouch(0).position;
                oldTouchPos2 = Input.GetTouch(1).position;
            }
            isSingleFinger = false;

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                ScaleCamera();
            }
        }
    }

    private void MoveCamera(Vector2 inputPos)
    {
        Vector3 lastTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(lastSingleTouchPos.x, lastSingleTouchPos.y, 0));
        Vector3 currentTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(inputPos.x, inputPos.y, 0));

        Camera.main.transform.position+= -(currentTouchPos- lastTouchPos);
        Camera.main.transform.position=new Vector3(Mathf.Clamp(Camera.main.transform.position.x, minPos.x, maxPos.x), Mathf.Clamp(Camera.main.transform.position.y, minPos.y, maxPos.y), -10);
        lastSingleTouchPos = inputPos;
    }

    private void ScaleCamera()
    {
        var tempPos1 = Input.GetTouch(0).position;
        var tempPos2 = Input.GetTouch(1).position;

        float lastTouchDistance = Vector3.Distance(oldTouchPos1,oldTouchPos2);
        float currentTouchDistance = Vector3.Distance(tempPos1, tempPos2);
        float distance = -((currentTouchDistance/2) - (lastTouchDistance/2));

        if (Camera.main.orthographicSize+ (distance * scaleFactor) > maxDistance)
            Camera.main.orthographicSize = maxDistance;
        else if (Camera.main.orthographicSize + (distance * scaleFactor) < minDistance)
            Camera.main.orthographicSize = minDistance;
        else
            Camera.main.orthographicSize += distance * scaleFactor;
        oldTouchPos1 = tempPos1;
        oldTouchPos2 = tempPos2;
    }
}
