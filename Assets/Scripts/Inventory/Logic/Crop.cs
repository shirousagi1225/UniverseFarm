using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public ItemName seedName;
    public ItemName cropName;

    public void CropClicked()
    {
        Harvest();
    }

    public void SetCrop(ItemDetails seedDetails, ItemName itemName)
    {
        //���ե�,����seedName��cropName���ȹ��
        seedName = itemName;
        cropName = seedDetails.itemName;
        GetComponent<SpriteRenderer>().sprite = seedDetails.itemSprite;
    }

    //�@��������k(������)
    private void Harvest()
    {
        InventoryManager.Instance.AddItem(cropName, seedName, FarmlandManager.Instance.Produce(seedName));
        Destroy(gameObject);
        transform.parent.GetComponent<Collider2D>().enabled = true;
    }

    //���A��T����ܤ�k(������)
}
