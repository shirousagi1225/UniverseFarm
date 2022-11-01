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
        InventoryManager.Instance.AddItem(itemName, seedName, FarmlandManager.Instance.Produce(seedName));
        Destroy(gameObject);
        transform.parent.GetComponent<Collider2D>().enabled = true;
    }

    //���A��T����ܤ�k(������)
}
