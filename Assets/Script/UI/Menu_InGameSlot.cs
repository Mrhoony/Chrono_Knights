﻿using UnityEngine;
using UnityEngine.UI;

public class Menu_InGameSlot : Slot
{
    public Image slotImage;
    public GameObject itemSelect;

    public void SetItemSprite(Sprite _slotSprite, Item _item)
    {
        slotImage.sprite = _slotSprite;
        if (_item == null)
        {
            itemImage.gameObject.SetActive(false);
            itemBorderImage.gameObject.SetActive(false);
        }
        else
        {
            itemImage.sprite = _item.sprite;
            itemImage.gameObject.SetActive(true);
            itemBorderImage.sprite = SpriteSet.keyItemBorderSprite[_item.itemRarity];
            itemBorderImage.gameObject.SetActive(true);
        }
    }

    public void SetItemSprite(Sprite _slotSprite, Item _item, bool _OnOff)
    {
        slotImage.sprite = _slotSprite;
        if (_item == null)
        {
            itemImage.gameObject.SetActive(false);
            itemBorderImage.gameObject.SetActive(false);
        }
        else
        {
            itemImage.sprite = _item.sprite;
            itemImage.gameObject.SetActive(true);
            itemBorderImage.sprite = SpriteSet.keyItemBorderSprite[_item.itemRarity];
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
        slotImage.sprite = _slotSprite;
        slotFocus.SetActive(false);
        itemImage.gameObject.SetActive(false);
        itemBorderImage.gameObject.SetActive(false);
        itemSelect.SetActive(false);
        itemConfirm.SetActive(false);
    }
}
