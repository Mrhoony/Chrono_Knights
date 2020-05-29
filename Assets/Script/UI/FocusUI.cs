using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusUI : MonoBehaviour
{
    public int focused;
    public int MaxFocused;

    public bool isUIOn;
    public GameObject cursor;
    public float cursorSpeed;

    public void FocusMove(GameObject _FocusedObject)
    {
        if (focused < 0 || focused > MaxFocused) return;
        cursor.transform.position = Vector2.Lerp(cursor.transform.position, _FocusedObject.transform.position, Time.deltaTime * cursorSpeed);
    }

    public void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue > MaxFocused) focused = 0;
        else if (focused + AdjustValue < 0) focused = MaxFocused;
        else focused += AdjustValue;

        cursor.SetActive(true);
    }
}
