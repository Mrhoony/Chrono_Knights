using UnityEngine;
using UnityEngine.UI;

public class Menu_InGameSlot : Slot
{
    public Image spriteRenderer;
    public GameObject itemSelect;

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
        itemConfirm.transform.GetChild(focus).gameObject.SetActive(false);
        itemConfirm.SetActive(false);

        return _isSelected;
    }
    public override void SetOverSlot(Sprite _slotSprite)
    {
        spriteRenderer.sprite = _slotSprite;
        slotFocus.SetActive(false);
        itemImage.gameObject.SetActive(false);
        itemBorderImage.gameObject.SetActive(false);
        itemSelect.SetActive(false);
        itemConfirm.SetActive(false);
    }
}
