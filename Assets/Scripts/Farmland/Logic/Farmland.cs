using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmland : MonoBehaviour
{
    public FarmlandName farmlandName;

    private SpriteRenderer farmlandSR;
    public SpriteRenderer smallIconSR;

    [HideInInspector]public bool isBackpackOpen = false;
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
            if (!isUnlock || isRepair || transform.childCount>1)
                return false;
            else
                return true;
        }
    }

    public void FarmlandClicked()
    {
        if (!isUnlock)
        {
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
        //���ޥΧP�_:�O�_���������B�ʶR��k
        isUnlock = true;
        isRepair = false;
        FarmlandManager.Instance.AddFarmland(farmlandName);
        smallIconSR.gameObject.SetActive(true);
        //���ե�,�����R��
        GetComponent<Collider2D>().enabled = false;
    }

    //��µ��k(������)

    public void PlantAction(GameObject mainCanvas)
    {
        isBackpackOpen = !isBackpackOpen;
        if (isBackpackOpen)
        {
            for (int i=0; i<mainCanvas.transform.childCount;i++)
            {
                mainCanvas.transform.GetChild(i).gameObject.SetActive(false);
            }
            mainCanvas.transform.GetChild(8).gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < mainCanvas.transform.childCount; i++)
            {
                mainCanvas.transform.GetChild(i).gameObject.SetActive(true);
            }
            mainCanvas.transform.GetChild(8).gameObject.SetActive(false);
        }
    }

    //���g�ܴ��p�ϥܪ��A��k(������)
}
