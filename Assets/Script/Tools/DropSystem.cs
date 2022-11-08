using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSystem : MonoBehaviour
{
    [SerializeField] private List<DropItem> dropItems;
    [SerializeField] private Transform dropPosition;

    public void CalculateDrops()
    {
        foreach (DropItem dropItem in dropItems)
        {
            if (Random.Range(0, 1) < dropItem.DropRate) 
            {
                Vector3 dropPos = dropPosition.position;
                dropPos.y += 0.5f;
                Instantiate(dropItem.ItemPrefab, dropPos, new Quaternion());
            }
        }
    }
}
