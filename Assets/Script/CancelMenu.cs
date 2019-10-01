using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelMenu : MonoBehaviour
{
    public GameObject SettingsMenu;

    public void OpenSettings()
    {
        SettingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        SettingsMenu.SetActive(false);
    }
}
