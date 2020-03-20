using UnityEngine;

public class TownUI_ShopSlot : Slot
{
    public override void SetItemSprite(Item _item, bool _OnOff)
    {
        Sprite[] keyItemBorderSprite = SpriteSet.shopItemBorderSprite;
        if (_item == null)
        {
            itemImage.gameObject.SetActive(false);
            itemBorderImage.gameObject.SetActive(false);
        }
        else
        {
            itemImage.sprite = _item.sprite;
            itemImage.gameObject.SetActive(true);
            itemBorderImage.sprite = keyItemBorderSprite[_item.itemRarity + 1];
            itemBorderImage.gameObject.SetActive(true);
        }
    }
    public override bool SetItemConfirm(bool _isSelected)
    {
        itemConfirm.transform.GetChild(focus).gameObject.SetActive(false);
        itemConfirm.SetActive(false);

        return _isSelected;
    }

    public override void SetOverSlot(Sprite _slotSprite)
    {
        slotFocus.SetActive(false);
        itemImage.gameObject.SetActive(false);
        itemBorderImage.gameObject.SetActive(false);
        itemConfirm.SetActive(false);
    }
}
