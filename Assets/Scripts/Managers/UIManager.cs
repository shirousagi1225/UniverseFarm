using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainCanvas;

    private bool canOpenSecUI = true;
    private bool isInfoBarOpen = false;
    private bool isSecUIOpen = false;

    public bool isMainUIOpen
    {
        set
        {
            canOpenSecUI = !value;
        }
    }

    //�i�ﵽ�G�z�L�ƥ����ӧOUI�i�H�ھڦۤv�ݨD�ϥΤ�k
    public void ShowInfoBar()
    {
        //���ե�,����true�אּ!isInfoBarOpen
        isInfoBarOpen = true;
        if (isInfoBarOpen)
        {
            mainCanvas.transform.GetChild(1).gameObject.SetActive(isInfoBarOpen);
        }
        else
        {
            mainCanvas.transform.GetChild(1).gameObject.SetActive(isInfoBarOpen);
        }
    }

    //���[�J�ѼƥΩ�P�_�Ӷ}�ҭ���UI
    //������u�}�Һؤl���������~��
    public void ShowSecUI()
    {
        if (canOpenSecUI)
        {
            isSecUIOpen = !isSecUIOpen;
            if (isSecUIOpen)
            {
                for (int i = 0; i < mainCanvas.transform.childCount; i++)
                {
                    mainCanvas.transform.GetChild(i).gameObject.SetActive(false);
                }
                mainCanvas.transform.GetChild(9).gameObject.SetActive(isSecUIOpen);
            }
            else
            {
                for (int i = 0; i < mainCanvas.transform.childCount; i++)
                {
                    mainCanvas.transform.GetChild(i).gameObject.SetActive(true);
                }
                mainCanvas.transform.GetChild(9).gameObject.SetActive(isSecUIOpen);
            }
        }
    }

    //���g�Ұ��ʶRUI��k
}