using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public ClientName clientName;

    private float speed;
    private Vector3 dir;//�B�ʤ�V
    private int randomIndex;//�O���H���I
    private int stateTime;
    private float countTime;//�w�ɾ�
    private bool isWalk;//���A�P�_

    private void Start()
    {
        CustomerManager.Instance.AddClient(clientName);
        //���C������@�Ӫ�l��V�A���L�h�������Ĳ�o��
        dir = new Vector3(Random.Range(0, 10), Random.Range(-10, 0), 0);
        countTime = 0;
        isWalk=true;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetCustomer(ClientDetails clientDetails)
    {
        clientName=clientDetails.clientName;
        GetComponent<SpriteRenderer>().sprite =clientDetails.clientSprite;
        speed = clientDetails.walkSpeed;
        stateTime = clientDetails.stateTime;
    }

    private void Move()
    {
        //�T�w��Ƨ��ܤ@�����A�A���C������i�H���������A���M�ӻ��w
        if (countTime > stateTime)
        {
            int stateValue = Random.Range(0, 2);

            if (stateValue == 1)
                isWalk = true;
            else
                isWalk = false;
            countTime = 0;//�w�ɾ��k�s
            //Debug.Log(isWalk);
        }
        else if (isWalk)
            transform.position += dir.normalized * speed * Time.deltaTime;

        countTime += Time.deltaTime;//�w��
    }

    //�ƥ��d���k�G�b����P��@�w�Z�����|�ƥ���L����,�קK���|(������)

    private void OnTriggerExit2D(Collider2D collision)
    {
        isWalk = false;
        //�b�H���I���H����@�ӥX��
        int temp = Random.Range(0, collision.transform.childCount);

        //�o���H���I�M�e�@���H���I����ۦP�A�p�G�ۦP���ܡA�N�|�����X���
        while (temp == randomIndex)
            temp = Random.Range(0, collision.transform.childCount);
        //���C������@�Ө��H���I����V�A�o�Ӥ�V�N�O�L���U�Ӫ��B�ʤ�V
        dir = collision.transform.GetChild(temp).position-transform.position;
        //������e�H���I
        randomIndex = temp;
        //Debug.Log(temp);
    }
}
