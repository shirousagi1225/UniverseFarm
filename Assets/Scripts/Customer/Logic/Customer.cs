using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public ClientName clientName;

    private float speed;
    private Vector3 dir;//運動方向
    private int randomIndex;//記錄隨機點
    private int stateTime;
    private float countTime;//定時器
    private bool isWalk;//狀態判斷

    private void Start()
    {
        CustomerManager.Instance.AddClient(clientName);
        //給遊戲物件一個初始方向，讓他去撞擊邊界觸發器
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

    //排斥範圍方法：在角色周圍一定距離內會排斥其他角色,避免重疊(未完成)

    private void OnTriggerExit2D(Collider2D collision)
    {
        isWalk = false;
        //在隨機點中隨機選一個出來
        int temp = Random.Range(0, collision.transform.childCount);

        //這個隨機點和前一個隨機點不能相同，如果相同的話，就會直接出邊界
        while (temp == randomIndex)
            temp = Random.Range(0, collision.transform.childCount);
        //給遊戲物件一個到隨機點的方向，這個方向就是他接下來的運動方向
        dir = collision.transform.GetChild(temp).position-transform.position;
        //紀錄當前隨機點
        randomIndex = temp;
        //Debug.Log(temp);
    }
}
