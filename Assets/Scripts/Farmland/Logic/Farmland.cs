using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmland : MonoBehaviour
{
    public FarmlandName farmlandName;

    private FarmlandState _currentState;//當前農田狀態
    private SpriteRenderer farmlandSR;

    private bool isUnlock;
    private bool isRepair;

    //判斷農田處於哪種狀態(用於維護：依據狀態可使用對應設施,並於處理後重設狀態)
    public FarmlandState currentState
    {
        get
        {
            return _currentState;
        }

        set
        {
            _currentState = value;
        }
    }

    private void Awake()
    {
        _currentState = FarmlandState.None;
        farmlandSR = GetComponent<SpriteRenderer>();
    }

    public bool canPlant
    {
        get
        {
            if (!isUnlock || isRepair || transform.childCount>3)
                return false;
            else
                return true;
        }
    }

    public void FarmlandClicked()
    {
        if (!isUnlock)
        {
            //須引用判斷:是否有足夠金額購買方法
            Unlock();
            //須呼叫變換農地狀態事件(解鎖)
            //呼叫變換農地狀態事件(可翻土)
            //EventHandler.CallUpdateFarmlandStateEvent(GetComponent<Farmland>());
        }
        else if (isRepair)
        {
            //突發事件系統相關
        }
    }

    //解鎖方法(未完成)
    public void Unlock()
    {
        isUnlock = true;
        isRepair = false;
        FarmlandManager.Instance.AddFarmland(farmlandName);
        //transform.GetChild(0).gameObject.SetActive(true);
        //測試用,正式刪除
        if(!canPlant)
            GetComponent<Collider2D>().enabled = false;
    }

    //修繕方法(未完成)

    public void PlantAction()
    {
        //需判斷是否翻過土,有翻過才能開啟種植UI
        UIManager.Instance.ShowBackpackBarUI();
    }

    //設定農田狀態
    public void SetFarmlandState(FarmlandState farmlandState)
    {
        _currentState = farmlandState;
    }
}
