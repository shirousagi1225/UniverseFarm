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

        //����ݼg�b�}�l�C���ƥ�
        var dialogueJson = UnityWebRequest.Get(dialogueJsonPath);
        dialogueJson.SendWebRequest();
        StartCoroutine(WaitLoadData(dialogueJson));
        Debug.Log(dialogueJson.downloadHandler.text);
        dialogueData_Ob = JsonMapper.ToObject<DialogueData_Ob>(Encoding.GetEncoding("utf-8").GetString(dialogueJson.downloadHandler.data));
        Debug.Log(dialogueData_Ob);

        dialogueStack = new Stack<string>();
        isTalking = false;
    }

    //Ū����ܤ奻��k
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

    //��ܹ�ܤ�k
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
            //�����lUI
            //�ݥ[�J�P�_�O�_�P�ɦ���L��UI�}��
            if(!GameObject.Find("UniversalPanel").activeInHierarchy)
                EventHandler.CallShowSecUIEvent(false, true);
            isTalking = false;
        }    
    }

    //���ݤU��������k
    private IEnumerator WaitLoadData(UnityWebRequest dialogueJson)
    {
        while(!dialogueJson.isDone)
            yield return 0;
    }
}
