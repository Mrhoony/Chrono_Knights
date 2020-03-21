using UnityEngine;
using UnityEngine.UI;

public class Menu_InGameSlot : Slot
{
    public Image spriteRenderer;
    public GameObject itemSelect;

    public void SetItemSprite(Item _item, bool _OnOff)
    {
        Sprite[] keyItemBorderSprite = SpriteSet.keyItemBorderSprite;

        if (_item == null)
        {
            itemImage.gameObject.SetActive(false);
            itemBorderImage.gameObject.SetActive(false);
        }
        else
        {
            itemImage.sprite = _item.sprite;
            itemImage.gameObject.SetActive(true);
            itemBorderImage.sprite = keyItemBorderSprite[_item.itemRarity];
            itemBorderImage.gameObject.SetActive(true);
        }
        itemSelect.SetActive(_OnOff);
    }

    public override bool SetItemConfirm(bool _isSelected)
    {
        if (_isSelected)
        {
            _isSelected = false;
            itemSelect.SetActive(false);
        }
        else
        {
            _isSelected = true;
            itemSelect.SetActive(true);
        }
        SetDisActiveItemConfirm();

        return _isSelected;
    }

    public void SetOverSlot(Sprite _slotSprite)
    {
        spriteRenderer.sprite = _slotSprite;
        slotFocus.SetActive(false);
        itemImage.gameObject.SetActive(false);
        itemBorderImage.gameObject.SetActive(false);
        itemSelect.SetActive(false);
        itemConfirm.SetActive(false);
    }
}
