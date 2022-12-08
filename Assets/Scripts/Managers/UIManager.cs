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
    private void OnShowSecUIEvent(bool canOpenSecUI, bool canSwitch)
    {
        if (canOpenSecUI)
        {
            ShowSecUI(canOpenSecUI, canSwitch);
        }
        else
        {
            CloseSecUI(canOpenSecUI, canSwitch);
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
                ShowSecUI(isBackpackBarUIOpen,true);
                SecCanvas.transform.GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);

                //����u�}�Һؤl���������~��
                SecCanvas.transform.GetChild(1).GetChild(0).gameObject.SetActive(!isBackpackBarUIOpen);
                SecCanvas.transform.GetChild(1).GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);
            }
            else
            {
                CloseSecUI(isBackpackBarUIOpen,true);
                SecCanvas.transform.GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);

                SecCanvas.transform.GetChild(1).GetChild(0).gameObject.SetActive(!isBackpackBarUIOpen);
                SecCanvas.transform.GetChild(1).GetChild(1).gameObject.SetActive(isBackpackBarUIOpen);
            }
        }
    }

    //���g�Ұ��ʶRUI��k

    private void ShowSecUI(bool toggleFactor, bool canSwitch)
    {
        //�P�_�O�n�����D��UI�γ�W�}�Ҧ�UI
        if (canSwitch)
        {
            mainCanvas.GetComponent<CanvasGroup>().alpha = 0f;
            mainCanvas.GetComponent<CanvasGroup>().interactable = !toggleFactor;
            mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = !toggleFactor;
        }
        SecCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        SecCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
        SecCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
    }

    private void CloseSecUI(bool toggleFactor, bool canSwitch)
    {
        //�P�_�O�n�����D��UI�γ�W�}�Ҧ�UI
        if (canSwitch)
        {
            mainCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
            mainCanvas.GetComponent<CanvasGroup>().interactable = !toggleFactor;
            mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = !toggleFactor;
        }
        SecCanvas.GetComponent<CanvasGroup>().alpha = 0f;
        SecCanvas.GetComponent<CanvasGroup>().interactable = toggleFactor;
        SecCanvas.GetComponent<CanvasGroup>().blocksRaycasts = toggleFactor;
    }
}
