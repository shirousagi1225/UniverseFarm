using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackUI : MonoBehaviour
{
    public Sprite startImage;
    public Sprite changeImage;

    private Image currentImage;
    private bool isBackpackOpen = false;

    private void Awake()
    {
        currentImage=GetComponent<Image>();
    }

    public void BackpackButton(GameObject backpackBar)
    {
        //���g�P�_�b�}�ҭI�]UI�ɨ�l����t�Υ\��Ҥ���ϥ�
        //���g�i���������������\��
        isBackpackOpen = !isBackpackOpen;
        UIManager.Instance.isMainUIOpen = isBackpackOpen;
        if (isBackpackOpen)
        {
            currentImage.sprite = changeImage;
            backpackBar.SetActive(true);
            backpackBar.transform.parent.GetComponent<CanvasGroup>().alpha = 1.0f;
            backpackBar.transform.parent.GetComponent<CanvasGroup>().interactable = isBackpackOpen;
            backpackBar.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = isBackpackOpen;
        }
        else
        {
            backpackBar.transform.parent.GetComponent<CanvasGroup>().alpha = 1.0f;
            backpackBar.transform.parent.GetComponent<CanvasGroup>().interactable = isBackpackOpen;
            backpackBar.transform.parent.GetComponent<CanvasGroup>().blocksRaycasts = isBackpackOpen;
            currentImage.sprite = startImage;
            backpackBar.SetActive(false);
        }
    }
}
