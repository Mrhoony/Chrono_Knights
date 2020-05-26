using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkBox : MonoBehaviour
{
    public Text dialogText;

    public void SetDialogText(string _Text)
    {
        dialogText.text = _Text;
    }
}
