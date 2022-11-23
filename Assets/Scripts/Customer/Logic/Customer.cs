using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Customer : MonoBehaviour
{
    public ClientName clientName;

    private float speed;
    private Vector3 dir;//運動方向
    private int randomIndex;//記錄隨機點
    private int stateTime;
    private float countTime;//定時器
    private bool isWalk;//狀態判斷

    private Vector3 startPos;
    private bool isDragging;
    private bool isExitRange;

    private void Start()
    {
        CustomerManager.Instance.AddClient(clientName);
        //給遊戲物件一個初始方向，讓他去撞擊邊界觸發器
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
        //判斷放開的地方是否為角色可移動之範圍,超出範圍則回到原先位置
        if (isExitRange)
        {
            //需判斷放開的地方是否為農田區以及是否有已成熟作物,有才能執行結帳方法
            //需在必要時開關農田區碰撞框
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

    //結帳方法(未完成)
    private bool CheckOut()
    {
        int soldCount= CustomerManager.Instance.BuyCrops(clientName);
        Debug.Log("購買數量：" + soldCount);

        //須加入結算UI方法
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

            //固定秒數改變一次狀態，讓遊戲物件可以停停走走，不然太僵硬
            if (countTime > stateTime)
            {
                int stateValue = Random.Range(0, 2);

                if (stateValue == 1)
                    isWalk = true;
                else
                    isWalk = false;
                countTime = 0;//定時器歸零
                              //Debug.Log(isWalk);
            }
            else if (isWalk)
                transform.position += dir.normalized * speed * Time.deltaTime;

            countTime += Time.deltaTime;//定時
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
                //在隨機點中隨機選一個出來
                int temp = Random.Range(0, collision.transform.childCount);

                //這個隨機點和前一個隨機點不能相同，如果相同的話，就會直接出邊界
                while (temp == randomIndex)
                    temp = Random.Range(0, collision.transform.childCount);
                //給遊戲物件一個到隨機點的方向，這個方向就是他接下來的運動方向
                dir = collision.transform.GetChild(temp).position - transform.position;
                //紀錄當前隨機點
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
