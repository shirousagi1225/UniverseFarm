using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Customer : MonoBehaviour
{
    public ClientName clientName;

    private float speed;
    private Vector3 dir;//�B�ʤ�V
    private int randomIndex;//�O���H���I
    private int stateTime;
    private float countTime;//�w�ɾ�
    private bool isWalk;//���A�P�_

    private Vector3 startPos;
    private bool isDragging;
    private bool isExitRange;

    private void Start()
    {
        CustomerManager.Instance.AddClient(clientName);
        //���C������@�Ӫ�l��V�A���L�h�������Ĳ�o��
        dir = new Vector3(Random.Range(0, 10), Random.Range(-10, 0), 0);
        countTime = 0;
        isWalk=true;
        isDragging=false;
        isExitRange=false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnMouseDrag()
    {
        if (!isDragging)
        {
            startPos = transform.position;
            isDragging = true;
        }
        //Debug.Log(isDragging);
        #if !UNITY_EDITOR && UNITY_ANDROID
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y,10));
        #else
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,10));
        #endif
    }

    private void OnMouseUp()
    {
        //�P�_��}���a��O�_������i���ʤ��d��,�W�X�d��h�^������m
        if (isExitRange)
        {
            //�ݧP�_��}���a��O�_���A�аϥH�άO�_���w�����@��,���~����浲�b��k
            //�ݦb���n�ɶ}���A�аϸI����
            if (Physics2D.OverlapCircle(transform.position, 2f, 1 << LayerMask.NameToLayer("InteractiveArea"))&& CheckOut())
            {
                Destroy(gameObject);
            }else
                transform.position = startPos;
            isExitRange=false;
        }
        isDragging = false;
    }

    public void SetCustomer(ClientDetails clientDetails)
    {
        clientName=clientDetails.clientName;
        GetComponent<SpriteRenderer>().sprite =clientDetails.clientSprite;
        speed = clientDetails.walkSpeed;
        stateTime = clientDetails.stateTime;
    }

    //���b��k(������)
    private bool CheckOut()
    {
        int soldCount= CustomerManager.Instance.BuyCrops(clientName);
        Debug.Log("�ʶR�ƶq�G" + soldCount);

        //���[�J����UI��k
        if (soldCount==0)
            return false;
        else
            return true;
    }

    private void Move()
    {
        if (!isDragging)
        {
            if (isWalk)
            {
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 2f, 1 << LayerMask.NameToLayer("Customer"));
                EventHandler.CallGetOtherCustomerEvent(transform, collider2Ds);
            }

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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetType().ToString() == "UnityEngine.PolygonCollider2D")
        {
            isWalk = false;
            if (isDragging)
                isExitRange = true;
            else
            {
                //�b�H���I���H����@�ӥX��
                int temp = Random.Range(0, collision.transform.childCount);

                //�o���H���I�M�e�@���H���I����ۦP�A�p�G�ۦP���ܡA�N�|�����X���
                while (temp == randomIndex)
                    temp = Random.Range(0, collision.transform.childCount);
                //���C������@�Ө��H���I����V�A�o�Ӥ�V�N�O�L���U�Ӫ��B�ʤ�V
                dir = collision.transform.GetChild(temp).position - transform.position;
                //������e�H���I
                randomIndex = temp;
                //Debug.Log(temp);
            }
            //Debug.Log(isExitRange);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
