using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintUI : MonoBehaviour
{
    //���g����UI�I����k(������)
    //�@�䦡���s�G�p�P�I�]UI,�I�}��|�̾ڮɶ����ǱƦC�U���A����,�Q�I���̮����Ҧ��Ӫ��A
    //���A���ܩw��G�I���Ӵ���,�N�۾��w��ܵo�ͦa
    public void HintButton()
    {
        foreach (var farmland in FindObjectsOfType<Farmland>())
        {
            if (farmland.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == transform.GetComponent<Image>().sprite)
                farmland.transform.GetChild(0).gameObject.SetActive(false);
            if (farmland.canPlant)
                EventHandler.CallUpdateFarmlandStateEvent(farmland);
        }
        gameObject.SetActive(false);
    }
}
