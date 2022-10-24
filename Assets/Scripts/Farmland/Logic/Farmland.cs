using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmland : MonoBehaviour
{
    public FarmlandName farmlandName;

    private SpriteRenderer spriteRenderer;
    private bool isUnlock;
    public bool isPlant;
    private bool isMaintain;

    private void Awake()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
    }

    public void FarmlandClicked()
    {
        if (!isUnlock)
        {
            //須啟動購買UI
            //須加入判斷:是否有足夠金額購買
            //須加入扣除金錢方法
            //須加入變換農地狀態方法
            isUnlock=true;
            isMaintain = false;
            isPlant=false;
            FarmlandManager.Instance.AddFarmland(farmlandName);
        }
        else if (isMaintain)
        {
            //突發事件系統相關
        }
    }

    public void PlantAction(ItemName seedName)
    {
        //須啟動種植作物UI
        //須啟動子物件
        isPlant = true;
        FarmlandManager.Instance.SetSeed(farmlandName, seedName);
    }

    //解鎖方法(未完成)

    //修繕方法(未完成)
}
