using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Text;
using System;
using UnityEngine.Networking;

public class DialogueManager : Singleton<DialogueManager>
{
    private Stack<string> dialogueStack;
    private DialogueData_Ob dialogueData_Ob;
    private string dialogueJsonPath= Application.streamingAssetsPath+ "/DialogueData/Json/Dialogs.json";
    private bool isTalking;
    private ClientDetails currentCustomer;

    protected override void Awake()
    {
        base.Awake();

        //之後需寫在開始遊戲事件
        var dialogueJson = UnityWebRequest.Get(dialogueJsonPath);
        dialogueJson.SendWebRequest();
        StartCoroutine(WaitLoadData(dialogueJson));
        Debug.Log(dialogueJson.downloadHandler.text);
        dialogueData_Ob = JsonMapper.ToObject<DialogueData_Ob>(Encoding.GetEncoding("utf-8").GetString(dialogueJson.downloadHandler.data));
        Debug.Log(dialogueData_Ob);

        dialogueStack = new Stack<string>();
        isTalking = false;
    }

    //讀取對話文本方法
    public void GetDialogueFormFile(ClientDetails clientDetails, string dialogueName)
    {
        currentCustomer = clientDetails;
        dialogueStack.Clear();

        var dialogue = dialogueData_Ob.dialogueData.Find(i => i.clientName == currentCustomer.clientName.ToString()).clientData.Find(i => i.dialogueName == dialogueName);

        for (int i= dialogue.dialogueContents.Length-1;i>-1;i--)
        {
            dialogueStack.Push(dialogue.dialogueContents[i]);
        }
    }

    //顯示對話方法
    public void ShowDialogue()
    {
        if(!isTalking)
            StartCoroutine(DialogueRoutine(dialogueStack));
    }

    private IEnumerator DialogueRoutine(Stack<string> data)
    {
        isTalking = true;
        if (data.TryPop(out string result))
        {
            EventHandler.CallShowDialogueEvent(currentCustomer, result);
            yield return null;
            isTalking = false;
        }
        else
        {
            EventHandler.CallShowDialogueEvent(currentCustomer, string.Empty);
            //關閉子UI
            //需加入判斷是否同時有其他次UI開啟
            if(!GameObject.Find("UniversalPanel").activeInHierarchy)
                EventHandler.CallShowSecUIEvent(false, true);
            isTalking = false;
        }    
    }

    //等待下載完成方法
    private IEnumerator WaitLoadData(UnityWebRequest dialogueJson)
    {
        while(!dialogueJson.isDone)
            yield return 0;
    }
}
