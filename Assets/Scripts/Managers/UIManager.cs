using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainCanvas;
    public GameObject secCanvas;
    public GameObject triCanvas;
    public GameObject hintButton;

    //�Ω�I�]��UI
    private bool canOpenBackpackBarUI = true;
    private bool isBackpackBarUIOpen = false;

    //�P�_�O�ѭ��ؤ覡�}�ҭI�]��UI(�Ω�I�]��UI�G�����I�]���s�Υi�شӹA�a)
    public bool isMainUIOpen
    {
        get { return !canOpenBackpackBarUI; }
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

    //�����DUI�P��UI�ƥ�
    private void OnShowSecUIEvent(string canvas,bool canOpenSecUI, bool canSwitch)
    {
        if (canOpenSecUI)
        {
            ShowSecUI(canvas,canOpenSecUI, canSwitch);
        }
        else
        {
            CloseSecUI(canvas,canOpenSecUI, canSwitch);
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

    //��ܭI�]��UI��k(������)
    public void ShowBackpackBarUI()
    {
        if (canOpenBackpackBarUI)
        {
            isBackpackBarUIOpen = !isBackpackBarUIOpen;
            if (isBackpackBarUIOpen)
            {
                ShowSecUI("SecCanvas",isBackpackBarUIOpen,true);
                secCanvas.transform.GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);

                //����u�}�Һؤl���������~��
                secCanvas.transform.GetChild(1).GetChild(0).gameObject.SetActive(!isBackpackBarUIOpen);
                secCanvas.transform.GetChild(1).GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);
            }
            else
            {
                CloseSecUI("SecCanvas", isBackpackBarUIOpen,true);
                secCanvas.transform.GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);

                secCanvas.transform.GetChild(1).GetChild(0).gameObject.SetActive(!isBackpackBarUIOpen);
                secCanvas.transform.GetChild(1).GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);
            }
        }
    }

    //���g�Ұ��ʶRUI��k

    private void ShowSecUI(string canvas,bool toggleFactor, bool canSwitch)
    {
        //�P�_�O�n�����D��UI�γ�W�}�Ҧ�UI
        if (canSwitch)
        {
            mainCanvas.GetComponent<CanvasGroup>().alpha = 0f;
            mainCanvas.GetComponent<CanvasGroup>().interactable = !toggleFactor;
            mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = !toggleFactor;
        }

        //�P�_�O�n�ϥέ��ئ�UI
        switch (canvas)
        {
            case "SecCanvas":
                secCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
                secCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
                secCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
                break;
            case "TriCanvas":
                triCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
                triCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
                triCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
                break;
        }
    }

    private void CloseSecUI(string canvas,bool toggleFactor, bool canSwitch)
    {
        //�P�_�O�n�����D��UI�γ�W�}�Ҧ�UI
        if (canSwitch)
        {
            mainCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
            mainCanvas.GetComponent<CanvasGroup>().interactable = !toggleFactor;
            mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = !toggleFactor;
        }

        //�P�_�O�n�ϥέ��ئ�UI
        switch (canvas)
        {
            case "SecCanvas":
                secCanvas.GetComponent<CanvasGroup>().alpha = 0f;
                secCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
                secCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
                break;
            case "TriCanvas":
                triCanvas.GetComponent<CanvasGroup>().alpha = 0f;
                triCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
                triCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
                break;
        }
    }
}
