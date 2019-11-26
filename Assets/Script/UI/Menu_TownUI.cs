using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_TownUI : MonoBehaviour
{
    public GameObject[] townMenus;

    public Menu_InGame menuIngame;

    private void Awake()
    {
        menuIngame = GameObject.Find("UI/Menus").GetComponent<Menu_InGame>();
    }
}
