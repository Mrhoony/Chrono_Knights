﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Control : MonoBehaviour
{
    public GameObject player;
    public MainUI_InGameMenu MainUI_InGameMenu;
    public bool inPlayer;

    public void Start()
    {
        player = GameObject.Find("PlayerCharacter");
        MainUI_InGameMenu = GameObject.Find("Menus").GetComponent<MainUI_InGameMenu>();
        inPlayer = false;
    }
}
