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
        //須寫判斷在開啟背包UI時其餘栽培系統功能皆不能使用
        //須寫可切換持有物種類功能
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
