using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Loot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.transform.GetChild(3).GetComponent<InventoryController>().Loot(gameObject))
            {
                Destroy(gameObject);
            }
        }
    }
}
