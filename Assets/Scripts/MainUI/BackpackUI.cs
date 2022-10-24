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
        isBackpackOpen = !isBackpackOpen;
        if (isBackpackOpen)
        {
            currentImage.sprite = changeImage;
            backpackBar.SetActive(true);
        }
        else
        {
            currentImage.sprite = startImage;
            backpackBar.SetActive(false);
        }
    }
}
