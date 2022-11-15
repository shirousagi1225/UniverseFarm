using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Seed : MonoBehaviour
{
    public ItemName seedName;
    public ItemName cropName;

    public void SeedClicked()
    {
        Collect();
    }

    //種子袋拾取方法
    private void Collect()
    {
        int count = Random.Range(1, 4);

        InventoryManager.Instance.AddSeed(seedName, cropName, count);
        Destroy(gameObject);
    }
}
