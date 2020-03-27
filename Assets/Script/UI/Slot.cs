using UnityEngine;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour
{
    public Image itemImage;
    public Image itemBorderImage;
    public GameObject itemConfirm;
    public GameObject leftText;
    public GameObject rightText;
    public int focus = 0;

    public void SetActiveItemConfirm(string _leftText, string _rightText)
    {
        focus = 0;
        itemConfirm.SetActive(true);
        itemConfirm.transform.GetChild(focus).gameObject.SetActive(true);
        leftText.SetActive(true);
        rightText.SetActive(true);
        leftText.GetComponent<Text>().text = _leftText;
        rightText.GetComponent<Text>().text = _rightText;
    }

    public void SetDisActiveItemConfirm()
    {
        itemConfirm.transform.GetChild(focus).gameObject.SetActive(false);
        focus = 0;
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

    public abstract bool SetItemConfirm(bool _isSelected);
}
