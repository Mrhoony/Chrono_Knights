using UnityEngine;
using UnityEngine.UI;

public class Menu_Slot : MonoBehaviour
{
    public Image spriteRenderer;
    public GameObject slotFocus;
    public Image itemImage;
    public Image itemBorderImage;
    public GameObject itemSelect;
    public GameObject itemConfirm;
    
    public int focus = 0;

    public void SetActiveFocus(bool _OnOff)
    {
        if (!_OnOff)
        {
            itemConfirm.SetActive(_OnOff);
        }
        slotFocus.SetActive(_OnOff);
    }
    public void SetItemSprite(Sprite _itemSprite, Sprite _itemBorderSprite, bool _OnOff)
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
    public void SetActiveItemConfirm(bool _OnOff)
    {
        itemConfirm.transform.GetChild(focus).gameObject.SetActive(_OnOff);
        if (!_OnOff) focus = 0;
        itemConfirm.SetActive(_OnOff);
    }
    public bool SetItemConfirm(bool _isSelected)
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
    public void SetOverSlot(Sprite _slotSprite)
    {
        spriteRenderer.sprite = _slotSprite;
        slotFocus.SetActive(false);
        itemImage.gameObject.SetActive(false);
        itemBorderImage.gameObject.SetActive(false);
        itemSelect.SetActive(false);
        itemConfirm.SetActive(false);
    }

    public void ItemConfirmFocus(int _adjustValue)
    {
        itemConfirm.transform.GetChild(focus).gameObject.SetActive(false);

        if (focus + _adjustValue < 0) focus = 1;
        else if (focus + _adjustValue > 1) focus = 0;
        else focus += _adjustValue;

        itemConfirm.transform.GetChild(focus).gameObject.SetActive(true);
    }
    public int GetFocus()
    {
        return focus;
    }
}
