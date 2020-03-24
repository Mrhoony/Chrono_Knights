using UnityEngine;
using UnityEngine.UI;

public class Menu_InGameSlot : Slot
{
    public Image slotImage;
    public GameObject itemSelect;

    public void SetItemSprite(Item _item)
    {
        slotImage.sprite = SpriteSet.inventorySprite[1];
        if (_item == null)
        {
            itemImage.gameObject.SetActive(false);
            itemBorderImage.gameObject.SetActive(false);
        }
        else
        {
            itemImage.sprite = _item.sprite;
            itemImage.gameObject.SetActive(true);
            itemBorderImage.sprite = SpriteSet.inventorySprite[6 - _item.itemRarity];
            itemBorderImage.gameObject.SetActive(true);
        }
    }
    public void SetItemSprite(Item _item, bool _OnOff)
    {
        slotImage.sprite = SpriteSet.inventorySprite[1];
        if (_item == null)
        {
            itemImage.gameObject.SetActive(false);
            itemBorderImage.gameObject.SetActive(false);
        }
        else
        {
            itemImage.sprite = _item.sprite;
            itemImage.gameObject.SetActive(true);
            itemBorderImage.sprite = SpriteSet.inventorySprite[6 - _item.itemRarity];
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

    public void SetOverSlot()
    {
        slotImage.sprite = SpriteSet.inventorySprite[2];
        itemImage.gameObject.SetActive(false);
        itemBorderImage.gameObject.SetActive(false);
        itemSelect.SetActive(false);
        itemConfirm.SetActive(false);
    }
}
