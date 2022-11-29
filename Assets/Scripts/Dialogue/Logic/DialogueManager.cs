using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
using System;
using UnityEngine.Networking;

public class DialogueManager : Singleton<DialogueManager>
{
    private Stack<string> dialogueStack;
    private DialogueData_Ob dialogueData_Ob;
    private string dialogueJsonPath= Application.streamingAssetsPath+ "/DialogueData/Json/Dialogs.json";
    private string dialogueFile;
    private bool isTalking;
    private ClientDetails currentCustomer;

    protected override void Awake()
    {
        base.Awake();
        #if !UNITY_EDITOR && UNITY_ANDROID
            //之後需寫在開始遊戲事件
            var dialogueJson = UnityWebRequest.Get(dialogueJsonPath);
            dialogueJson.SendWebRequest();

            dialogueData_Ob = JsonMapper.ToObject<DialogueData_Ob>(dialogueJson.downloadHandler.text);
        #else
            var dialogueJson = File.ReadAllText(dialogueJsonPath, Encoding.GetEncoding("utf-8"));
            dialogueData_Ob = JsonMapper.ToObject<DialogueData_Ob>(dialogueJson);
        #endif

        dialogueStack = new Stack<string>();
        isTalking = false;
    }

    //讀取對話文本方法(未完成)
    public void GetDialogueFormFile(ClientDetails clientDetails, string dialogueName)
    {
        currentCustomer = clientDetails;
        dialogueStack.Clear();
        //Debug.Log(dialogueFile);

        var dialogue = dialogueData_Ob.dialogueData.Find(i => i.clientName == currentCustomer.clientName.ToString()).clientData.Find(i => i.dialogueName == dialogueName);

        for (int i= dialogue.dialogueContents.Length-1;i>-1;i--)
        {
            dialogueStack.Push(dialogue.dialogueContents[i]);
        }
    }

    //顯示對話方法(未完成)
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
            EventHandler.CallShowSecUIEvent(false);
            isTalking = false;
        }    
    }
}
