using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfomation : MonoBehaviour
{
    public GameObject itemSprite;
    public Text itemName;
    public Text itemRarity;
    public Text itemEffect;
    public Text itemEquipEffect;

    public void SetItemInfomation(Item item)
    {
        itemSprite.GetComponent<Image>().sprite = item.sprite;
        itemName.text = item.itemName;
        itemRarity.text = item.itemRarity.ToString();
        itemEffect.text = item.usingType.ToString();
    }

    public void SetItemInventoryInformation(Item item)
    {

    }
}
