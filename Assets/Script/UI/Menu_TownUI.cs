using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_TownUI : MonoBehaviour
{
    public GameObject[] townMenus;

    public MainUI_Menu menu;

    private void Awake()
    {
        menu = GameObject.Find("UI/Menus").GetComponent<MainUI_Menu>();
    }
}
