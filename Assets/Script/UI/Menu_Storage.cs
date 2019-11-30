using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Storage : MonoBehaviour
{
    public MainUI_InGameMenu menu;

    private void OnEnable()
    {
        menu = GameObject.Find("UI/Menus").GetComponent<MainUI_InGameMenu>();
        menu.OpenInventory(5);
    }
    private void OnDisable()
    {
        menu.CloseInventory();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
