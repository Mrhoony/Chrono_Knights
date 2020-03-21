using UnityEngine;
using UnityEngine.UI;

public class TownUI_ShopSlot : Slot
{
    public GameObject itemCost;

    public void SetItemSprite(Item _item, int gold)
    {
        if (_item == null)
        {
            itemImage.gameObject.SetActive(false);
            itemBorderImage.gameObject.SetActive(false);
            itemCost.SetActive(false);
        }
        else
        {
            itemImage.sprite = _item.sprite;
            itemImage.gameObject.SetActive(true);
            itemBorderImage.sprite = SpriteSet.shopItemBorderSprite[_item.itemRarity + 1];
            itemBorderImage.gameObject.SetActive(true);
            itemCost.SetActive(true);
            itemCost.GetComponent<Text>().text = gold.ToString() + " gold";
        }
    }
    public override bool SetItemConfirm(bool _isSelected)
    {
        itemConfirm.transform.GetChild(focus).gameObject.SetActive(false);
        itemConfirm.SetActive(false);

        return _isSelected;
    }
}
