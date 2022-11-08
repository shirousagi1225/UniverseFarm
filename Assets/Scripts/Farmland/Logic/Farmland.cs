using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmland : MonoBehaviour
{
    public FarmlandName farmlandName;

    private SpriteRenderer farmlandSR;
    public SpriteRenderer smallIconSR;

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
            //���[�J�ܴ��A�a���A��k
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
        smallIconSR.gameObject.SetActive(true);
        //���ե�,�����R��
        GetComponent<Collider2D>().enabled = false;
    }

    //��µ��k(������)

    public void PlantAction()
    {
        UIManager.Instance.ShowSecUI();
    }

    //���g�ܴ��p�ϥܪ��A��k(������)
}
