using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatDialog : MonoBehaviour
{
    public Text dialogName;
    public Text dialogText;

    public void SetDialogText(string _Name, string _Text)
    {
        dialogName.text = _Name;
        dialogText.text = _Text;
    }
}
