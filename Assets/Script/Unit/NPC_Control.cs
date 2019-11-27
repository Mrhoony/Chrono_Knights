using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Control : MonoBehaviour
{
    public GameObject player;
    public MainUI_Menu MainUI_Menu;
    public bool inPlayer;

    public void Start()
    {
        player = GameObject.Find("PlayerCharacter");
        MainUI_Menu = GameObject.Find("Menus").GetComponent<MainUI_Menu>();
        inPlayer = false;
    }
}
