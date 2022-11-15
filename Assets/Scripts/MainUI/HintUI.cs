using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintUI : MonoBehaviour
{
    //須寫提示UI點擊方法(未完成)
    //一鍵式按鈕：如同背包UI,點開後會依據時間順序排列各狀態提示,被點擊者消除所有該狀態
    //狀態提示定位：點擊該提示,將相機定位至發生地
    public void HintButton()
    {
        foreach (var farmland in FindObjectsOfType<Farmland>())
        {
            if (farmland.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == transform.GetComponent<Image>().sprite)
                farmland.transform.GetChild(0).gameObject.SetActive(false);
            if (farmland.canPlant)
                EventHandler.CallUpdateFarmlandStateEvent(farmland);
        }
        gameObject.SetActive(false);
    }
}
