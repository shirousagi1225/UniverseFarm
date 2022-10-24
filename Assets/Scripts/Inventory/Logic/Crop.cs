using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public ItemName itemName;
    public ItemName seedName;

    public void CropClicked()
    {
        Harvest();
    }

    //�@��������k(������)
    private void Harvest()
    {
        InventoryManager.Instance.AddItem(itemName, FarmlandManager.Instance.Produce(seedName));
        Destroy(gameObject);
    }

    //���A��T����ܤ�k(������)
}
