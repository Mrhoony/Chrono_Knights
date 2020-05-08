using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatDialog : MonoBehaviour
{
    public Text dialogName;
    public Text dialogText;
    public List<EventDialog> eventDialog;

    public void SetDialogText(List<EventDialog> _EventDialog)
    {
        eventDialog = _EventDialog;
    }
}
