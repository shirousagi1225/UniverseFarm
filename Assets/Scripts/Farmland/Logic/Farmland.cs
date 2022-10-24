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
            //���Ұ��ʶRUI
            //���[�J�P�_:�O�_���������B�ʶR
            //���[�J����������k
            //���[�J�ܴ��A�a���A��k
            isUnlock=true;
            isMaintain = false;
            isPlant=false;
            FarmlandManager.Instance.AddFarmland(farmlandName);
        }
        else if (isMaintain)
        {
            //��o�ƥ�t�ά���
        }
    }

    public void PlantAction(ItemName seedName)
    {
        //���Ұʺشӧ@��UI
        //���Ұʤl����
        isPlant = true;
        FarmlandManager.Instance.SetSeed(farmlandName, seedName);
    }

    //�����k(������)

    //��µ��k(������)
}
