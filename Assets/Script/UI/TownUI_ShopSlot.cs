using UnityEngine;

public class TownUI_ShopSlot : Slot
{
    public override void SetItemSprite(Sprite _itemSprite, Sprite _itemBorderSprite, bool _OnOff)
    {
        if (_itemSprite == null)
        {
            itemImage.gameObject.SetActive(false);
            itemBorderImage.gameObject.SetActive(false);
        }
        else
        {
            itemImage.sprite = _itemSprite;
            itemImage.gameObject.SetActive(true);
            itemBorderImage.sprite = _itemBorderSprite;
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
