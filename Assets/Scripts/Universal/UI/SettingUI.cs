using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public GameObject panel;

    public void SettingButton()
    {
        //啟動子UI
        EventHandler.CallShowSecUIEvent("SecCanvas", true, false);
        panel.SetActive(true);
    }

    public void CloseSettingUI()
    {
        panel.SetActive(false);
        EventHandler.CallShowSecUIEvent("SecCanvas", false, false);
    }

    //清除資料方法：清除資料並關閉遊戲
    public void ResetData()
    {
        StartCoroutine(SaveLoadManager.Instance.Remove());
        #if !UNITY_EDITOR && UNITY_ANDROID
            Application.Quit();
        #else
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
