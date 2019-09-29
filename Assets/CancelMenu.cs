using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelMenu : MonoBehaviour
{
    public GameObject SettingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenSettings()
    {
        SettingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        SettingsMenu.SetActive(false);
    }
}
