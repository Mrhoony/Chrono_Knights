using UnityEngine;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour
{
    public GameObject slotFocus;
    public Image itemImage;
    public Image itemBorderImage;
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

    public void SetActiveItemConfirm(bool _OnOff)
    {
        itemConfirm.transform.GetChild(focus).gameObject.SetActive(_OnOff);
        if (!_OnOff) focus = 0;
        itemConfirm.SetActive(_OnOff);
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

    public abstract void SetItemSprite(Item _item, bool _OnOff);
    public abstract bool SetItemConfirm(bool _isSelected);
    public abstract void SetOverSlot(Sprite _slotSprite);
}
