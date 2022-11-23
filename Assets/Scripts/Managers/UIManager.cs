using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainCanvas;
    public GameObject hintButton;

    private bool canOpenSecUI = true;
    private bool isSecUIOpen = false;

    public bool isMainUIOpen
    {
        set
        {
            canOpenSecUI = !value;
        }
    }

    private void OnEnable()
    {
        EventHandler.UpdateHintUIEvent += OnUpdateHintUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateHintUIEvent -= OnUpdateHintUIEvent;
    }

    //�ȩw(�i�ק�)�G��s����UI�ƥ�,�έp�U���A���ܥX�{�ƶq(�{���X�h���ܦҼ{��W�g�@�Ӹ}��)
    private void OnUpdateHintUIEvent(Farmland farmland, Sprite farmlandStateSprite)
    {
        //�ݰѷ�InventoryUI�}��,�ʺA��s����UI���U���A���ܼƶq�ζ���
        if (!hintButton.activeInHierarchy)
            hintButton.SetActive(true);

        hintButton.GetComponent<Image>().sprite = farmlandStateSprite;
    }

    //�i�ﵽ�G�z�L�ƥ����ӧOUI�i�H�ھڦۤv�ݨD�ϥΤ�k
    //���[�J�ѼƥΩ�P�_�Ӷ}�ҭ���UI
    //�ݧP�_�}�Һش�UI�O�I���A�a,�����h�O�I������(�u�����I�����~�|����,���i��۾����ʫh���|)
    public void ShowSecUI()
    {
        if (canOpenSecUI)
        {
            isSecUIOpen = !isSecUIOpen;
            if (isSecUIOpen)
            {
                for (int i = 0; i < mainCanvas.transform.childCount; i++)
                {
                    if(mainCanvas.transform.GetChild(i).name!= "HintButton")
                        mainCanvas.transform.GetChild(i).gameObject.SetActive(false);
                }
                mainCanvas.transform.GetChild(9).gameObject.SetActive(isSecUIOpen);
                //����u�}�Һؤl���������~��
                mainCanvas.transform.GetChild(9).GetChild(0).gameObject.SetActive(!isSecUIOpen);
                mainCanvas.transform.GetChild(9).GetChild(1).gameObject.SetActive(isSecUIOpen);
            }
            else
            {
                for (int i = 0; i < mainCanvas.transform.childCount; i++)
                {
                    if (mainCanvas.transform.GetChild(i).name != "HintButton")
                        mainCanvas.transform.GetChild(i).gameObject.SetActive(true);
                }
                mainCanvas.transform.GetChild(9).GetChild(0).gameObject.SetActive(!isSecUIOpen);
                mainCanvas.transform.GetChild(9).GetChild(1).gameObject.SetActive(isSecUIOpen);
                mainCanvas.transform.GetChild(9).gameObject.SetActive(isSecUIOpen);
                
            }
        }
    }

    //���g�Ұ��ʶRUI��k
}
