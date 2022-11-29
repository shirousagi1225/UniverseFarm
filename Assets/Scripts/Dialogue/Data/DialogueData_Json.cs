using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可嘗試使用字典減少class數量
public class DialogueData_Json
{
    public string dialogueName;
    public string[] dialogueContents;
}

public class ClientData_Json
{
    public string clientName;
    public List<DialogueData_Json> clientData;
}

public class DialogueData_Ob
{
    public List<ClientData_Json> dialogueData;
}