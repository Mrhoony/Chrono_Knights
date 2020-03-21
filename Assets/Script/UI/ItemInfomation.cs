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
    public Text itemDescription;

    public void SetItemInventoryInformation(Item item)
    {
        if(item != null)
        {
            itemName.text = item.itemName;
            itemRarity.text = "Rarity : " + item.itemRarity.ToString();
            itemEffect.text = item.usingType.ToString();
            itemEquipEffect.text = item.itemType.ToString();
            itemDescription.text = item.Description;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetItemInformationQuickSlot(Item item)
    {
        if (item != null)
        {
            itemSprite.GetComponent<Image>().sprite = item.sprite;
            itemName.text = item.itemName;
            itemRarity.text = "Rarity : " + item.itemRarity.ToString();
            itemEffect.text = item.usingType.ToString();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
