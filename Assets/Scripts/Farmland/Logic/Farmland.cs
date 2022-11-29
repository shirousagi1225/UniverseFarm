using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmland : MonoBehaviour
{
    public FarmlandName farmlandName;

    private SpriteRenderer farmlandSR;

    private bool isUnlock;
    private bool isRepair;

    private void Awake()
    {
        farmlandSR = GetComponent<SpriteRenderer>();
    }

    public bool canPlant
    {
        get
        {
            if (!isUnlock || isRepair || transform.childCount>2)
                return false;
            else
                return true;
        }
    }

    public void FarmlandClicked()
    {
        if (!isUnlock)
        {
            //���ޥΧP�_:�O�_���������B�ʶR��k
            Unlock();
            //���I�s�ܴ��A�a���A�ƥ�(����)
            //�I�s�ܴ��A�a���A�ƥ�(�i½�g)
            EventHandler.CallUpdateFarmlandStateEvent(GetComponent<Farmland>());
        }
        else if (isRepair)
        {
            //��o�ƥ�t�ά���
        }
    }

    //�����k(������)
    public void Unlock()
    {
        isUnlock = true;
        isRepair = false;
        FarmlandManager.Instance.AddFarmland(farmlandName);
        transform.GetChild(0).gameObject.SetActive(true);
        //���ե�,�����R��
        if(!canPlant)
            GetComponent<Collider2D>().enabled = false;
    }

    //��µ��k(������)

    public void PlantAction()
    {
        //�ݧP�_�O�_½�L�g,��½�L�~��}�Һش�UI
        UIManager.Instance.ShowBackpackBarUI();
    }
}
