using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainCanvas;
    public GameObject SecCanvas;
    public GameObject hintButton;

    //�Ω�I�]��UI
    private bool canOpenBackpackBarUI = true;
    private bool isBackpackBarUIOpen = false;

    //�Ω�I�]��UI
    public bool isMainUIOpen
    {
        set{ canOpenBackpackBarUI = !value; }
    }

    private void OnEnable()
    {
        EventHandler.ShowSecUIEvent += OnShowSecUIEvent;
        EventHandler.UpdateHintUIEvent += OnUpdateHintUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.ShowSecUIEvent -= OnShowSecUIEvent;
        EventHandler.UpdateHintUIEvent -= OnUpdateHintUIEvent;
    }

    //�ݫ�ҭn�ǤJ����Ѽ�(�D�n�\��G�}���lUI ��s��r ��ܸӮ��ӫ~�ƶq ��ܸӮ��ӫ~�Ϥ�)
    private void OnShowSecUIEvent(bool canOpenSecUI)
    {
        if (canOpenSecUI)
        {
            ShowSecUI(canOpenSecUI);
        }
        else
        {
            CloseSecUI(canOpenSecUI);
        }
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
    public void ShowBackpackBarUI()
    {
        if (canOpenBackpackBarUI)
        {
            isBackpackBarUIOpen = !isBackpackBarUIOpen;
            if (isBackpackBarUIOpen)
            {
                ShowSecUI(isBackpackBarUIOpen);
                SecCanvas.transform.GetChild(0).gameObject.SetActive(isBackpackBarUIOpen);

                //����u�}�Һؤl���������~��
                SecCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(!isBackpackBarUIOpen);
                SecCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);
            }
            else
            {
                CloseSecUI(isBackpackBarUIOpen);
                SecCanvas.transform.GetChild(0).gameObject.SetActive(isBackpackBarUIOpen);

                SecCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(!isBackpackBarUIOpen);
                SecCanvas.transform.GetChild(0).GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);
            }
        }
    }

    //���g�Ұ��ʶRUI��k

    private void ShowSecUI(bool toggleFactor)
    {
        mainCanvas.GetComponent<CanvasGroup>().alpha = 0f;
        mainCanvas.GetComponent<CanvasGroup>().interactable = !toggleFactor;
        mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = !toggleFactor;
        SecCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        SecCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
        SecCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
    }

    private void CloseSecUI(bool toggleFactor)
    {
        mainCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        mainCanvas.GetComponent<CanvasGroup>().interactable = !toggleFactor;
        mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = !toggleFactor;
        SecCanvas.GetComponent<CanvasGroup>().alpha = 0f;
        SecCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
        SecCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
    }
}
