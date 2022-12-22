using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    public GameObject panel;

    public void SettingButton()
    {
        //�ҰʤlUI
        EventHandler.CallShowSecUIEvent("SecCanvas", true, false);
        panel.SetActive(true);
    }

    public void CloseSettingUI()
    {
        panel.SetActive(false);
        EventHandler.CallShowSecUIEvent("SecCanvas", false, false);
    }

    //�M����Ƥ�k�G�M����ƨ������C��
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
