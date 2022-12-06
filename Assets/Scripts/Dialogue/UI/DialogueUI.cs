using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject panel;
    public Image customer;
    public Text customerName;
    public Text content;

    public float textSpeed;
    private bool isInit;
    private bool isDialogueFn;
    private bool canTyping;//取消逐字顯示

    private void OnEnable()
    {
        isInit=false;
        isDialogueFn=true;
        canTyping=true;
        EventHandler.ShowDialogueEvent += OnShowDialogueEvent;
    }

    private void OnDisable()
    {
        EventHandler.ShowDialogueEvent -= OnShowDialogueEvent;
    }

    private void Update()
    {
        //判斷是否點擊以繼續進行對話
        if (isDialogueFn && Input.GetMouseButtonDown(0) && GameObject.Find("DialoguePanel") != null)
            DialogueManager.Instance.ShowDialogue();
        else if (!isDialogueFn && canTyping && Input.GetMouseButtonDown(0) && GameObject.Find("DialoguePanel") != null)
            canTyping = false;
    }

    private void OnShowDialogueEvent(ClientDetails clientDetails, string dialogue)
    {
        if (dialogue != string.Empty)
            panel.SetActive(true);
        else
        {
            isInit = false;
            panel.SetActive(false);
        }
            
        if (!isInit&& panel.activeInHierarchy)
        {
            isInit = true;
            customer.sprite= clientDetails.clientSprite;
            customer.SetNativeSize();
            customerName.text = clientDetails.clientName.ToString();
        }
        StartCoroutine(SetDialogueUI(dialogue));
    }

    //逐字顯示對話方法
    private IEnumerator SetDialogueUI(string dialogue)
    {
        isDialogueFn=false;
        content.text = "";
        int letter = 0;

        while(canTyping&& letter< dialogue.Length - 1)
        {
            content.text += dialogue[letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        content.text = dialogue;
        canTyping=true;
        isDialogueFn = true;
    }
}
